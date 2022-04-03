using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface IPuzzleService
    {
        public Task<Puzzle> GetPuzzle(int id);

        public Task<Puzzle> FindPuzzleInfo(string barcode);

        public Task<Puzzle> CreatePuzzle(string barcode, string name, int pieceCount);
    }
}
