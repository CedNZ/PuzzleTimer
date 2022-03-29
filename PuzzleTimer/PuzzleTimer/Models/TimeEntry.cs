using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public User User { get; set; }
        public SolvingSession SolvingSession { get; set; }
        public Puzzle Puzzles { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
