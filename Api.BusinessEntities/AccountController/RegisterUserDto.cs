using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities.AccountController
{
    public class RegisterUserReq
    {
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// When true, the user needs to setup the app Google Authenticator in his phone. 
        /// <para> This app generates a code every few seconds. </para> 
        /// <para> This code will be requried by the user while performing sensitive tasks like making payments.</para> 
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
    }

    public class RegisterUserRes
    {
        /// <summary>
        /// The user must use this key to configure Google Authenticator app. This is a one time process.
        /// <para>Once done, the user will have to enter the code generated in the app for performing sensitive tasks e.g. transactions etc.</para>
        /// </summary>
        public string PrivateSharedKey { get; set; }

        /// <summary>
        /// Represents a userfriendly message.
        /// </summary>
        public string Message { get; set; }
    }
}
