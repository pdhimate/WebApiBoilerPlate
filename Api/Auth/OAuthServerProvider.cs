using Api.Data;
using Api.Data.Access.Repositories.Security;
using Api.Data.Models;
using Api.Data.Models.Security;
using Api.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Auth
{
    /// <summary>
    /// Customized OAuth server.
    /// </summary>
    public class OAuthServerProvider : OAuthAuthorizationServerProvider
    {
        #region Const fields

        /// <summary>
        /// Custom key that indicates the allowed origin by the client in the request headers.
        /// This is used while receiveing a refresh token, which further generates a new access token.
        /// </summary>
        internal const string Client_AllowedOriginKey = "as:clientAllowedOrigin";

        /// <summary>
        /// Custrom key that indicates the life time of the refresh token.
        /// This is sent as a ticket property after validating a client.
        /// This is used while generating refresh tokens.
        /// </summary>
        internal const string Client_RefreshTokenLifeTimeKey = "as:clientRefreshTokenLifeTime";

        /// <summary>
        /// Custom key corresponsing to the client id. 
        /// This is sent as a ticket property after validating a client.
        /// This is used while generating refresh access token.
        /// </summary>
        internal const string ClientIdKey = "as:client_id";

        #endregion

        #region Public overridden methods

        /// <summary>
        /// Validates the UserName and Password specified in the context and issues an access token containing the claims.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Read allowed origing value from client and add it to the response header
            var allowedOrigin = context.OwinContext.Get<string>(Client_AllowedOriginKey);
            if (allowedOrigin == null) allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            // Validate user credentials
            var userManager = context.OwinContext.GetUserManager<AppUserManager>();
            AppUser user = await userManager.FindAsync(context.UserName, context.Password);

            // User was not found.
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.Rejected();
                // TODO: log attempts with invalid credentials.
                return;
            }

            // Email not confirmed
            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "Your email id has not been confirmed yet.");
                context.Rejected();
                return;
            }

            // Fetch user claims. This adds all the user claims and roles to the access token.
            ClaimsIdentity oAuthIdentity = await AppUserManager.GenerateUserIdentityAsync(user,
                                                                                          userManager,
                                                                                          OAuthDefaults.AuthenticationType);
            AddCustomDataToClaims(userManager, user, oAuthIdentity);

            // Generate access token and validate the request with it.
            AuthenticationProperties properties = GetAuthProperties(context);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary[ClientIdKey];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            // Try to get the Client id and secret from the authorization header using a basic scheme 
            // So one way to send the client_id/client_secret is to base64 encode the (client_id:client_secret) 
            // and send it in the Authorization header
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                // The other way is to sent the client_id/client_secret as “x-www-form-urlencoded”.
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "ClientId must be sent.");
                return Task.FromResult<object>(null);
            }

            client = GetClientFromDb(context.ClientId);

            // Client is not registered to consume our back end api
            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format($"Client '{context.ClientId}' is not registered in the system."));
                return Task.FromResult<object>(null);
            }

            // Reject disabled clients
            if (!client.IsActive)
            {
                context.SetError("invalid_clientId", "Client is disabled.");
                return Task.FromResult<object>(null);
            }

            // Sending client secret is mandatory for native clients
            if (client.ClientType == ClientType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Encryptor.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            context.OwinContext.Set(Client_AllowedOriginKey, client.AllowedOrigin);
            context.OwinContext.Set(Client_RefreshTokenLifeTimeKey, client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        private static Client GetClientFromDb(string clientId)
        {
            Client client;
            using (var appDbContext = new AppDatabaseContext())
            {
                client = new ClientRepo(appDbContext).GetById(clientId);
            }

            return client;
        }

        // TODO: explore the usage of this method. 
        // Most probably it is used while using external login provider 
        // and to avoi man in the middle attack.
        // Refer: https://github.com/tjoudeh/AngularJSAuthentication/blob/master/AngularJSAuthentication.API/Controllers/AccountController.cs#L276
        // http://stackoverflow.com/questions/20693082/setting-the-redirect-uri-in-asp-net-identity/26276646#26276646
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            var clientId = context.ClientId;
            var client = GetClientFromDb(clientId);

            if (client == null)
            {
                context.Rejected();
                context.SetError($"Client Id [{clientId}] is not registered in the system.");
            }

            var redirectUri = new Uri(context.RedirectUri);
            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                context.Rejected();
                context.SetError($"The given url is not allowed by client id [{clientId}] ");
            }

            Uri expectedRootUri = new Uri(context.Request.Uri, "/");
            if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Match endpoint is called before Validate Client Authentication. 
        /// We need to allow the clients based on domain to enable requests the header
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            // TODO: After implementing client-based allow origin, not sure if this is needed anymore. 
            // Because we are already setting the CORS headers while granting access token.
            //SetCorsPolicy(context.OwinContext);
            //if (context.Request.Method == "OPTIONS")
            //{
            //    context.RequestCompleted();
            //    return Task.FromResult(0);
            //}

            return base.MatchEndpoint(context);
        }

        #endregion

        #region Private helpers

        private static void AddCustomDataToClaims(AppUserManager userManager, AppUser user, ClaimsIdentity oAuthIdentity)
        {
            // Add Two factor PSK claim, only if enabled
            if (user.TwoFactorEnabled)
            {
                oAuthIdentity.AddClaim(new Claim(AppUserClaims.TwoFactorPskClaimKey, user.TwoFactorPreSharedKey, ClaimValueTypes.String));
            }
        }

        /// <summary>
        /// Add the allow-origin header only if the origin domain is found on the     
        /// allowedOrigin list
        /// </summary>
        /// <param name="context"></param>
        private void SetCorsPolicy(IOwinContext context)
        {
            var allowedUrls = new List<string> { "*" };

            string origin = context.Request.Headers.Get("Origin") ?? string.Empty;
            var allowAll = allowedUrls.Any(url => url == "*");
            var found = allowedUrls.Any(url => url == origin);
            if (allowAll || found)
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", new string[] { origin });
            }

            context.Response.Headers.Add("Access-Control-Allow-Headers", new string[] { "Authorization", "Content-Type" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new string[] { "OPTIONS", "POST" });
        }

        private static AuthenticationProperties GetAuthProperties(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { ClientIdKey, (context.ClientId == null) ? string.Empty : context.ClientId },
                { "userName", context.UserName }
            };
            AuthenticationProperties properties = new AuthenticationProperties(data);
            return properties;
        }

        #endregion


    }
}