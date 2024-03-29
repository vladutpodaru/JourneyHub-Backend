using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Context
{
    public class CommandDbContext : JourneyDbContext
    {
        public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
        {
            //ChangeTracker.StateChanged += TrackChanges;
            //ChangeTracker.Tracked += TrackChanges;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        //private static void TrackChanges(object sender, EntityEntryEventArgs e)
        //{
        //    if (e.Entry.Entity is BaseEntity model)
        //    {
        //        var result = e.Entry.State switch
        //        {
        //            EntityState.Deleted => model.Delete(),
        //            EntityState.Modified => model.Update(),
        //            EntityState.Added => model.Create(),
        //            _ => default,
        //        };
        //    }
        //}
    }
}
