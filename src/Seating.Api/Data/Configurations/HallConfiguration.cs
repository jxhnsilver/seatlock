using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seating.Api.Entities;

namespace Seating.Api.Data.Configurations
{
    public sealed class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.ToTable("halls");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                .HasColumnName("id");

            builder.Property(h => h.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(h => h.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(h => h.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
