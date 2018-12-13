using System.ComponentModel.DataAnnotations;

namespace Api.BusinessEntities.AccountController
{
    public class UserExistsReq
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
    }
}
