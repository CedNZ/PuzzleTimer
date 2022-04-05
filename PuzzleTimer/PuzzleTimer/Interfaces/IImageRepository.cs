using System.Collections.Generic;
using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface IImageRepository
    {
        public Task<Image> AddImage(Image image);
        public Task<Image> GetImage(int id);
        public Task<IEnumerable<Image>> GetImagesForPuzzle(int puzzleId);
        public Task<IEnumerable<Image>> GetImagesForSession(int sessionId);
        public Task DeleteImage(int id);
    }
}
