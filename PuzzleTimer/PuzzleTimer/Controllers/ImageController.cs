using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet(nameof(GetImage), Name = nameof(GetImage))]
        public async Task<Image> GetImage([FromQuery] int id)
        {
            return await _imageService.GetImage(id);
        }

        [HttpGet(nameof(GetPuzzleImages), Name = nameof(GetPuzzleImages))]
        public async Task<IEnumerable<Image>> GetPuzzleImages([FromQuery] int puzzleId)
        {
            return await _imageService.GetImagesForPuzzle(puzzleId);
        }

        [HttpGet(nameof(GetSessionImages), Name = nameof(GetSessionImages))]
        public async Task<IEnumerable<Image>> GetSessionImages([FromQuery] int sessionId)
        {
            return await _imageService.GetImagesForSession(sessionId);
        }

        [HttpPost(nameof(AddImage), Name = nameof(AddImage))]
        public async Task<Image> AddImage([FromBody] ImageDTO imageDTO)
        {
            return await _imageService.AddImage(imageDTO.FileName, imageDTO.PuzzleId, imageDTO.Base64Image, imageDTO.SessionId);
        }

        [HttpDelete(nameof(DeleteImage), Name = nameof(DeleteImage))]
        public async Task DeleteImage(int id)
        {
            await _imageService.DeleteImage(id);
            return;
        }
    }

    public class ImageDTO
    {
        public string FileName { get; set; }
        public int PuzzleId { get; set; }
        public string Base64Image { get; set; }
        public int? SessionId { get; set; }
    }
}
