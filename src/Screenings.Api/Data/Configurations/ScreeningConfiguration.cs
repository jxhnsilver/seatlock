using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Screenings.Api.Domain;

namespace Screenings.Api.Data.Configurations
{
    public sealed class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
    {
        public void Configure(EntityTypeBuilder<Screening> builder)
        {
            builder.ToTable("screenings");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.MovieId)
                .HasColumnName("movie_id")
                .IsRequired();

            builder.Property(s => s.HallId)
                .HasColumnName("hall_id")
                .IsRequired();

            builder.Property(s => s.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            builder.Property(s => s.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            builder.Property(s => s.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(s => new { s.HallId, s.StartTime })
                .HasDatabaseName("ix_screenings_hall_start_time");
        }
    }
}