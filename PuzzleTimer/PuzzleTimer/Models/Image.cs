using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuzzleTimer.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public Puzzle Puzzle { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? SortOrder { get; set; }

        [NotMapped]
        public string Base64 { get; set; }

#nullable enable
        public SolvingSession? SolvingSession { get; set; }
#nullable restore
    }
}
