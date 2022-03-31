using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface IPuzzleService
    {
        public Task<Puzzle> GetPuzzle(int id);

        public Task<Puzzle> FindPuzzleInfo(string barcode);

        public Task<Puzzle> CreatePuzzle(string barcode, string name, int pieceCount);
    }
}
