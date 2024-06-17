using OfficeSync.Domain.Entities;
using OfficeSync.Domain.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeSync.Domain.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Email { get; set; }
        bool EmailConfirmed { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool TwoFactorEnabled { get; set; }
        MfaProvider DefaultMfaProvider { get; set; }
        string LastUpdatedBy { get; set; }

        [NotMapped]
        UserEventActivity EventActivity { get; set; }
        [NotMapped]
        string Link { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string RoleName { get; set; }

        UserProfile ProfileRef { get; set; }
    }
}
