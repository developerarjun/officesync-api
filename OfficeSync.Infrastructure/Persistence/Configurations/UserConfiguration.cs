using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeSync.Infrastructure.Persistence.Identity;

namespace OfficeSync.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(h => h.Id);
            builder.HasOne(h => h.ProfileRef)
                .WithOne(w => (User)w.UserRef)
                .HasForeignKey<User>(h => h.Id)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(300)
                .IsRequired(true);
            builder.HasIndex(h => h.Email)
                .IsUnique();

            builder.Property(p => p.LastUpdatedBy)
                .HasMaxLength(300)
                .IsRequired(true);
        }
    }
}
