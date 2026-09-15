using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seating.Api.Entities;

namespace Seating.Api.Data.Configurations
{
    public sealed class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.ToTable("seats");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.HallId)
                .HasColumnName("hall_id")
                .IsRequired();

            builder.Property(s => s.RowNumber)
                .HasColumnName("row_number")
                .IsRequired();

            builder.Property(s => s.SeatNumber)
                .HasColumnName("seat_number")
                .IsRequired();

            builder.Property(s => s.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne<Hall>()
                .WithMany()
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            // Для поиска всех мест конкретного зала.
            builder.HasIndex(s => s.HallId)
                .HasDatabaseName("ix_seats_hall_id");

            // В одном зале не может быть двух мест с одинаковым рядом и номером.
            builder.HasIndex(s => new { s.HallId, s.RowNumber, s.SeatNumber })
                .HasDatabaseName("ix_seats_hall_row_seat")
                .IsUnique();
        }
    }
}
