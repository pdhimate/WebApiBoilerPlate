using Api.Data;
using Api.Data.Access.Repositories.Security;
using Api.Data.Models.Security;
using Api.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Api.Auth
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            Task.WaitAll(CreateAsync(context));
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary[OAuthServerProvider.ClientIdKey];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");
            using (var dbContext = new AppDatabaseContext())
            {
                var repo = new RefreshTokenRepo(dbContext);
                var refreshTokenLifeTime = context.OwinContext.Get<string>(OAuthServerProvider.Client_RefreshTokenLifeTimeKey);
                long userId = -1;
                long.TryParse(context.Ticket.Identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out userId);

                var token = new RefreshToken()
                {
                    Id = Encryptor.GetHash(refreshTokenId), // stored hashed values only
                    ClientId = clientid,
                    UserId = userId,
                    IssuedOnUtc = DateTime.UtcNow,
                    ExpiresOnUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedOnUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresOnUtc;

                token.ProtectedTicket = context.SerializeTicket();

                repo.Insert(token);
                repo.SaveChanges();
                context.SetToken(refreshTokenId);
            }

            await Task.FromResult<object>(null);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            Task.WaitAll(ReceiveAsync(context));
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(OAuthServerProvider.Client_AllowedOriginKey);
            if (allowedOrigin == null) allowedOrigin = "*"; // TODO: investigate why this value is null.
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Encryptor.GetHash(context.Token);

            using (var dbContext = new AppDatabaseContext())
            {
                var repo = new RefreshTokenRepo(dbContext);
                {
                    var refreshToken = repo.GetById(hashedTokenId);
                    if (refreshToken != null)
                    {
                        //Get protectedTicket from refreshToken class
                        context.DeserializeTicket(refreshToken.ProtectedTicket);
                        repo.Delete(hashedTokenId);
                        repo.SaveChanges();
                    }
                }
            }

            await Task.FromResult<object>(null);
        }
    }
}