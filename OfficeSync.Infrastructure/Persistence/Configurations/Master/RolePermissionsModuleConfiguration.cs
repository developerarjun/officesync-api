using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeSync.Domain.Entities.Master;
using OfficeSync.Infrastructure.Persistence.Identity;

namespace OfficeSync.Infrastructure.Persistence.Configurations.Master
{
    public class RolePermissionsModuleConfiguration : IEntityTypeConfiguration<RolePermissionsModule>
    {
        public void Configure(EntityTypeBuilder<RolePermissionsModule> builder)
        {
        
            builder.HasOne(h => (Role)h.RoleRef)
               .WithMany()
               .HasForeignKey(h => h.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
