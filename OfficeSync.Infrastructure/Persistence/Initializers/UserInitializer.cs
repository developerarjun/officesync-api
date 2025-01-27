﻿using Microsoft.EntityFrameworkCore;
using OfficeSync.Application.Common.Helpers;
using OfficeSync.Domain.Entities.Master;
using OfficeSync.Infrastructure.Persistence.Identity;

namespace OfficeSync.Infrastructure.Persistence.Initializers
{
    public class UserInitializer : BaseInitializer
    {
        public UserInitializer(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public const int SUPER_ADMIN_ID = 1;
        public const string SUPER_EMAIL = "developerarjun1@gmail.com";
        public void SeedUsers()
        {
            var lastUpdatedAt = new DateTimeOffset(new DateTime(2023, 2, 28, 10, 10, 58, 959, DateTimeKind.Unspecified).AddTicks(6954), new TimeSpan(0, 0, 0, 0, 0));

            var dbSuperAgentProfile = new UserProfile
            {
                Id = SUPER_ADMIN_ID,
                FirstName = "Super",
                LastName = "Agent",
                LastUpdatedAt = lastUpdatedAt,
                LastUpdatedBy = "SA",
                IsActive = true
            };
            _modelBuilder.Entity<UserProfile>().HasData(dbSuperAgentProfile);

            var dbSuerAgentUser = new User
            {
                Id = SUPER_ADMIN_ID,
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

            var dbUserRole = new UserRole { UserId = SUPER_ADMIN_ID, RoleId = RoleInitializer.SUPER_ADMIN };
            _modelBuilder.Entity<UserRole>().HasData(dbUserRole);

            SeedUsersFromJSON().GetAwaiter().GetResult();
        }

        public async Task<List<UserSeedData>> GetJsonUsersDataToSeed()
        {
            var usersToSeed = await FileHelper.ReadJsonFile<List<UserSeedData>>("AdminUsersSeedData.json", "Persistence\\Initializers\\SeedData");

            return usersToSeed;
        }
        public async Task SeedUsersFromJSON()
        {
            var usersToSeed = await GetJsonUsersDataToSeed();
            if (usersToSeed is not null && usersToSeed.Count() > 0)
            {
                usersToSeed.ForEach(e =>
                {
                    var userProfile = e.UserProfile;
                    var user = e.User;
                    userProfile.Id= user.Id;
                    userProfile.LastUpdatedAt = new DateTimeOffset(DateTime.UtcNow);

                    _modelBuilder.Entity<UserProfile>().HasData(userProfile);

                    user.NormalizedEmail = user.Email.ToUpper();
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.LastUpdatedAt = new DateTimeOffset(DateTime.UtcNow);
                    _modelBuilder.Entity<User>().HasData(user);

                    var userRole = new UserRole { UserId = user.Id, RoleId = e.UserRole };
                    _modelBuilder.Entity<UserRole>().HasData(userRole);
                });
            }
        }

    }

    public class UserSeedData
    {
        public UserProfile UserProfile { get; set; }
        public User User { get; set; }
        public int UserRole { get; set; }//TODO: Handle it through ENUM or something better that this
    }
}
