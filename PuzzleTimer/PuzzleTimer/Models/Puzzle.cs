using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Models
{
    public class Puzzle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int PieceCount { get; set; }
    }
}
