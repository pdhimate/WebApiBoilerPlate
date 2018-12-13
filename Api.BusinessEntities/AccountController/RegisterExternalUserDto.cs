using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.AccountController
{
    /// <summary>
    /// Dto to register for users of external OAuth service providers like facebook, google to sign up.
    /// </summary>
    public class RegisterExternalUserReq
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
