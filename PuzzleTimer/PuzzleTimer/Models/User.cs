using System.Collections.Generic;

namespace PuzzleTimer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SolvingSession> SolvingSessions { get; set; }
    }
}
