using System;

namespace PuzzleTimer.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public User User { get; set; }
        public SolvingSession SolvingSession { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Comment { get; set; }
    }
}
