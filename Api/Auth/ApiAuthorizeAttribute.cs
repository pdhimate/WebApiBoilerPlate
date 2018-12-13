using Api.Data.Models;
using Api.Data.Models.Security;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Api.Auth
{
    /// <summary>
    /// Provides for a filter to Authorize with Two factor Otp. 
    /// Use this attribute to perform sensitive tasks like making a payment.
    /// Note: The user would be required to enter the time-based OTP such tasks.
    /// </summary>
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Peform the bear token authentication first
            base.OnAuthorizationAsync(actionContext, cancellationToken);

            // Perform authentication of the two factor code.
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            var preSharedKeyClaim = principal.FindFirst(AppUserClaims.TwoFactorPskClaimKey);

            // A TwoFactorPskClaim would only exist if two factor authentication is enabled.
            var twoFactorAuthenticationEnabled = preSharedKeyClaim != null;
            if (twoFactorAuthenticationEnabled)
            {
                var preSharedKey = preSharedKeyClaim.Value;
                bool hasValidTotp = OtpHelper.HasValidTotp(actionContext.Request, preSharedKey);
                if (!hasValidTotp)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "One Time Password is Invalid");
                }
            }
            return Task.FromResult<object>(null);
        }
    }

}