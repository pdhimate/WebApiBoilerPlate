using Api.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers
{
    /// <summary>
    /// Provides common methods for any <see cref="ApiController"/>
    /// </summary>
    public abstract class BaseApiController : ApiController
    {
        #region Protected Properties

        private AppUserManager _appUserManager;
        protected AppUserManager AppUserManager
        {
            get
            {
                if (_appUserManager == null)
                {
                    _appUserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                }
                return _appUserManager;
            }
        }

        #endregion

        public BaseApiController()
        {
        }

        #region Public methods

        /// <summary>
        /// Creates a response with BadRequest containing errors from the specified idenityResult.
        /// </summary>
        /// <param name="identityResult"></param>
        /// <returns>BadRequest</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult BadRequestWithIdentityErrors(IdentityResult identityResult)
        {
            if (identityResult == null)
            {
                throw new ArgumentNullException($"{nameof(identityResult)} cannot be null.");
            }

            if (identityResult.Succeeded || identityResult.Errors == null)
            {
                throw new InvalidOperationException($"{nameof(identityResult)} has no errors.");
            }

            var errors = new List<string>();
            foreach (string error in identityResult.Errors)
            {
                errors.Add(error);
            }

            var errorString = string.Join(Environment.NewLine, errors);
            return BadRequest(errorString);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Gets the Id of the user associated with the access token. (logged in user)
        /// </summary>
        /// <returns></returns>
        protected long GetUserIdFromContext()
        {
            return User.Identity.GetUserId<long>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _appUserManager != null)
            {
                _appUserManager.Dispose();
                _appUserManager = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}