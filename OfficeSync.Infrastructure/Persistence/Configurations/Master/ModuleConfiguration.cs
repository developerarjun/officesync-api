using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeSync.Domain.Entities.Master;

namespace OfficeSync.Infrastructure.Persistence.Configurations.Master
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.Property(h => h.Name)
                .HasMaxLength(300)
                .IsRequired(true);
        }
    }
}
