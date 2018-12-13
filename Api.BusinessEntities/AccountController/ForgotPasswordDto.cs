using System.ComponentModel.DataAnnotations;

namespace Api.BusinessEntities.AccountController
{
    public class ForgotPasswordReq
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
