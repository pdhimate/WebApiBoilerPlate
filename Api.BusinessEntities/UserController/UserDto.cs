namespace Api.BusinessEntities.UserController
{
    public class UserResDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public long? AddressId { get; set; }
    }
}
