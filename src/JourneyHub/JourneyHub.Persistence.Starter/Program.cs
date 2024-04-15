using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace JourneyHub.Persistence.Starter
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            // "-2" return code for when no parameter is given
            if (args.Length == 0)
            {
                Console.WriteLine("No parameter has been found.");
                return -2;
            }

            var dbConnectionString = args[0];

            var regex = new Regex(@"^User ID=(.*);Password=(.*);Host=(.*);Port=([0-9])\w+;Database=([A-Za-z])\w+;Command Timeout=([0-9])\w+$");
            var regexMatch = regex.Match(dbConnectionString);

            // "-1" return code for when parameter is not valid
            if (!regexMatch.Success)
            {
                Console.WriteLine($"The connection string {dbConnectionString} is not valid.");
                return -1;
            }

            var builder = new DbContextOptionsBuilder<OptContext>().UseNpgsql(dbConnectionString);

            var dbCtx = new OptContext(builder.Options);
            dbCtx.Database.Migrate();

            Console.WriteLine("The database has been successfully migrated.");
            return 0;
        }
    }
}
