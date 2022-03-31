using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System.Threading.Tasks;

namespace PuzzleTimer.Services
{
    public class PuzzleService : IPuzzleService
    {
        private readonly IPuzzleRepository _puzzleRepository;

        public PuzzleService(IPuzzleRepository puzzleRepository)
        {
            _puzzleRepository = puzzleRepository;
        }

        public async Task<Puzzle> GetPuzzle(int id)
        {
            var puzzle = await _puzzleRepository.GetPuzzle(id);

            return puzzle;
        }

        public Task<Puzzle> FindPuzzleInfo(string barcode)
        {
            return _puzzleRepository.FindPuzzle(barcode);
        }
    }
}
