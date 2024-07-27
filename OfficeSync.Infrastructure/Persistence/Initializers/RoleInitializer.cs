using Microsoft.EntityFrameworkCore;
using OfficeSync.Infrastructure.Persistence.Identity;

namespace OfficeSync.Infrastructure.Persistence.Initializers
{
    public class RoleInitializer : BaseInitializer
    {
        public RoleInitializer(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public const int SUPER_ADMIN = 1;
        public const string SUPER_ADMIN_NAME = "Super Admin";
        public void SeedRoles()
        {
            var dbSuperAgent = new Role { Id = SUPER_ADMIN, Name = SUPER_ADMIN_NAME, ConcurrencyStamp = "647808af-878a-41e5-9d69-5796165214bd", NormalizedName = SUPER_ADMIN_NAME.ToUpper()};

            _modelBuilder.Entity<Role>().HasData(dbSuperAgent);
        }
    }
}
