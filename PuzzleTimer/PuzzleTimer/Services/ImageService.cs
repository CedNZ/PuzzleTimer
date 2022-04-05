using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IPuzzleRepository _puzzleRepository;
        private readonly ISolvingSessionRepository _sessionRepository;

        public ImageService(IImageRepository imageRepository, IPuzzleRepository puzzleRepository, ISolvingSessionRepository sessionRepository)
        {
            _imageRepository = imageRepository;
            _puzzleRepository = puzzleRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<Image> AddImage(string filename, int puzzleId, string base64img, int? sessionId)
        {
            var matches = Regex.Match(base64img, "data:image.*base64,");

            base64img = Regex.Replace(base64img, "data:image.*base64,", "");

            var puzzle = await _puzzleRepository.GetPuzzle(puzzleId);

            var image = new Image
            {
                CreatedOn = DateTime.Now,
                FileName = filename,
                Puzzle = puzzle,
                Base64 = base64img,
            };

            if (sessionId.HasValue)
            {
                image.SolvingSession = await _sessionRepository.GetSolvingSession(sessionId.Value);
            }

            image = await _imageRepository.AddImage(image);
            image.Base64 = string.Concat(matches.Captures[0].Value, image.Base64);
            return image;
        }

        public async Task DeleteImage(int id)
        {
            await _imageRepository.DeleteImage(id);
            return;
        }

        public async Task<Image> GetImage(int id)
        {
            return await _imageRepository.GetImage(id);
        }

        public async Task<IEnumerable<Image>> GetImagesForPuzzle(int puzzleId)
        {
            return await _imageRepository.GetImagesForPuzzle(puzzleId);
        }

        public async Task<IEnumerable<Image>> GetImagesForSession(int sessionId)
        {
            return await _imageRepository.GetImagesForSession(sessionId);
        }
    }
}
