using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Database.Context
{
    public class JourneyDbContext(DbContextOptions options) : DbContext(options)
    {
        #region -- DB SETS --
        // public virtual DbSet<Entity> Entity { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // foreach entity
            // modelBuilder.Entity<Entity>().ToTable($"EntName_{nameof(EntityName)}");
            // modelBuilder.ApplyConfiguration(new EntityConfiguationName());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
