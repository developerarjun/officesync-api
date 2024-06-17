using Microsoft.EntityFrameworkCore;
using OfficeSync.Domain.Entities;
using OfficeSync.Infrastructure.Persistence.Identity;

namespace OfficeSync.Infrastructure.Persistence.Initializers
{
    public class UserInitializer : BaseInitializer
    {
        public UserInitializer(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public const int SUPER_AGENT_ID = 1;
        public const string SUPER_EMAIL = "developerarjun1@gmail.com";
        public void SeedUsers()
        {
            var lastUpdatedAt = new DateTimeOffset(new DateTime(2023, 2, 28, 10, 10, 58, 959, DateTimeKind.Unspecified).AddTicks(6954), new TimeSpan(0, 0, 0, 0, 0));

            var dbSuperAgentProfile = new UserProfile
            {
                Id = SUPER_AGENT_ID,
                FirstName = "Super",
                LastName = "Agent",
                LastUpdatedAt = lastUpdatedAt,
                LastUpdatedBy = "SA",
                IsActive = true
            };
            _modelBuilder.Entity<UserProfile>().HasData(dbSuperAgentProfile);

            var dbSuerAgentUser = new User
            {
                Id = SUPER_AGENT_ID,
                UserName = SUPER_EMAIL,
                NormalizedUserName = SUPER_EMAIL.ToUpper(),
                Email = SUPER_EMAIL,
                NormalizedEmail = SUPER_EMAIL.ToUpper(),
                EmailConfirmed = true,
                ConcurrencyStamp = "b91d74d8-5516-4cfb-b7e5-02d3885cb2bd",
                SecurityStamp = "851536ae-aded-4c9d-b342-738d0fb066eb",
                LastUpdatedAt = lastUpdatedAt,
                LastUpdatedBy = "SA",
                LockoutEnabled = true,
                IsActive = true
            };
            _modelBuilder.Entity<User>().HasData(dbSuerAgentUser);

            var dbUserRole = new UserRole { UserId = SUPER_AGENT_ID, RoleId = RoleInitializer.SUPER_AGENT };
            _modelBuilder.Entity<UserRole>().HasData(dbUserRole);
        }
    }
}
