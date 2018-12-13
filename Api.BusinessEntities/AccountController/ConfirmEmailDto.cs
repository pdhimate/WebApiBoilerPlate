using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.AccountController
{
    public class ConfirmEmailReq
    {
        [Required]
        public string Code { get; set; }
    }
}
