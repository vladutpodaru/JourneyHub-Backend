using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Context
{
    public class QueryDbContext: JourneyDbContext
    {
        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {

        }
    }
}
