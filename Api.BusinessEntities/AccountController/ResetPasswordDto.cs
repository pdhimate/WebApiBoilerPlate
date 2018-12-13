using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.AccountController
{
    public class ResetPasswordReq
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmed new password do not match.")]
        public string ConfirmNewPassword { get; set; }

        /// <summary>
        /// The password reset token received in the email.
        /// </summary>
        [Required]
        public string PasswordResetToken { get; set; }
    }
}
