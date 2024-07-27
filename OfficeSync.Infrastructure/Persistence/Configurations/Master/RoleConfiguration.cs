using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeSync.Domain.Entities.Master;
using OfficeSync.Infrastructure.Persistence.Identity;

namespace OfficeSync.Infrastructure.Persistence.Configurations.Master
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasOne(h => h.ParentRef)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}
