using Microsoft.EntityFrameworkCore;
using Screenings.Api.Domain;

namespace Screenings.Api.Data
{
    public sealed class ScreeningDbContext : DbContext
    {
        public ScreeningDbContext(DbContextOptions<ScreeningDbContext> options)
            : base(options) { }

        public DbSet<Screening> Screenings =>
            Set<Screening>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScreeningDbContext).Assembly);
        }
    }
}
