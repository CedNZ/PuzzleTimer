﻿using Microsoft.EntityFrameworkCore;

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
            optionsBuilder.UseSqlServer("Server=.; Database=PuzzleTimer; Integrated Security=SSPI; TrustServerCertificate=True;");
        }
    }
}
