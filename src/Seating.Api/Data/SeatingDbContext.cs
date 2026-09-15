using Microsoft.EntityFrameworkCore;
using Seating.Api.Entities;

namespace Seating.Api.Data
{
    public sealed class SeatingDbContext : DbContext
    {
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }

        public SeatingDbContext(DbContextOptions<SeatingDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SeatingDbContext).Assembly);
        }
    }
}
