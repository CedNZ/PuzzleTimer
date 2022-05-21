using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PuzzleTimer.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<SolvingSession> SolvingSessions { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Image> Images { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.; Database=PuzzleTimer; Integrated Security=SSPI; TrustServerCertificate=True;");
            optionsBuilder.UseSqlite("Data Source=PuzzleTimerDB.db;",
                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.AmbientTransactionWarning));
        }
    }
}
