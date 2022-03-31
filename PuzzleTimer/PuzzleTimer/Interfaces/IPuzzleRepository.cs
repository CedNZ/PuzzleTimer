using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface IPuzzleRepository
    {
        public Task<Puzzle> GetPuzzle(int id);

        public Task<Puzzle> FindPuzzle(string barcode);

        public Task<int> AddPuzzle(Puzzle puzzle);
    }
}
