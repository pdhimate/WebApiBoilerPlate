using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.BusinessEntities.AccountController
{
    /// <summary>
    /// Represents a user profile that can be fetched and updated.
    /// Note: This acts as both Request and response dto for different Api calls.
    /// </summary>
    public class ProfileDto
    {
        [MaxLength(70)]
        public string FirstName { get; set; }
        [MaxLength(70)]
        public string MiddleName { get; set; }
        [MaxLength(70)]
        public string LastName { get; set; }

        [MaxLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The roles associated with this user.
        /// </summary>
        public List<string> Roles { get; set; }
    }

    /// <summary>
    /// Represents/Indicates an updated user profile.
    /// </summary>
    public class ProfileRes
    {

    }
}
