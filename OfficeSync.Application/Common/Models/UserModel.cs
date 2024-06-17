namespace OfficeSync.Application.Common.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public bool IsMfaEnabled { get; set; }
        public string Role { get; set; }
    }
}
