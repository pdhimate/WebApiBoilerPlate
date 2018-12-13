using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.AccountController
{
  public  class ConfirmPhoneNumberReq
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string SmsCode { get; set; }
    }
}
