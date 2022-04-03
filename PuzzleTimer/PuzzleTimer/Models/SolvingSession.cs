using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Models
{
    public class SolvingSession
    {
        public int Id { get; set; }
        public Puzzle Puzzle { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public List<TimeEntry> TimeEntries { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Completed { get; set; }
    }
}
