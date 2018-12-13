using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api.Auth.Validators
{
    /// <summary>
    /// Configures validation logic for passwords.
    /// </summary>
    public class AppPasswordValidator : PasswordValidator
    {
        public AppPasswordValidator()
        {
            RequiredLength = 6;
            RequireNonLetterOrDigit = true;
            RequireDigit = false;
            RequireLowercase = true;
            RequireUppercase = true;
        }
    }
}