using Microsoft.EntityFrameworkCore;

namespace Screening.Api.Data
{
    public sealed class ScreeningDbContext : DbContext
    {
        public ScreeningDbContext(DbContextOptions<ScreeningDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScreeningDbContext).Assembly);
        }
    }
}
