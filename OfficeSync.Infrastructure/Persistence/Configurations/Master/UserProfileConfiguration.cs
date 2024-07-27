using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeSync.Domain.Entities.Master;

namespace OfficeSync.Infrastructure.Persistence.Configurations.Master
{
    public class UserProfileConfiguration : BaseConfiguration<UserProfile>
    {
        public override void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Suffix)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(p => p.FirstName)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(p => p.MiddleName)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(p => p.LastName)
                .HasMaxLength(300)
                .IsRequired(false);
        }
    }
}
