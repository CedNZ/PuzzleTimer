using System.Collections.Generic;
using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface IPuzzleRepository
    {
        public Task<Puzzle> GetPuzzle(int id);

        public Task<Puzzle> FindPuzzle(string barcode);

        public Task<int> AddPuzzle(Puzzle puzzle);

        public Task<IEnumerable<Puzzle>> FindPuzzlesByName(string name);
    }
}
