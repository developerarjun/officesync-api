using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeSync.Domain.Entities.Master;

namespace OfficeSync.Infrastructure.Persistence.Configurations.Master
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<int>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn(1, 1)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.HasKey(h => h.Id);

            builder.Property(p => p.LastUpdatedBy)
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}
