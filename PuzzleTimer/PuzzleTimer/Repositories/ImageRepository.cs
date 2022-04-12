using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private const string BASE_DIR = @"/tmp/PuzzleTimer/";
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public ImageRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Image> AddImage(Image image)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var path = GetImagePath(image);

                var bytes = Convert.FromBase64String(image.Base64);

                var saveTask = File.WriteAllBytesAsync(path, bytes);

                ctx.Entry(image).State = EntityState.Added;

                if (await ctx.SaveChangesAsync() == 1)
                {
                    await saveTask;

                    return image;
                }
                return null;
            }
        }

        public async Task DeleteImage(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var image = await ctx.Images
                    .Include(x => x.Puzzle)
                    .FirstOrDefaultAsync(i => i.Id == id);

                ctx.Remove(image);

                await ctx.SaveChangesAsync();

                var path = GetImagePath(image);

                File.Delete(path);

                return;
            }
        }

        public async Task<Image> GetImage(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var image = await ctx.Images
                    .Include(i => i.Puzzle)
                    .FirstOrDefaultAsync(i => i.Id == id);
                if (image != null)
                {
                    return PopulateImageInfo(image);
                }
                return null;
            }
        }

        public async Task<IEnumerable<Image>> GetImagesForPuzzle(int puzzleId)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var images = await ctx.Images
                    .Include(i => i.Puzzle)
                    .Include(i => i.SolvingSession)
                    .Where(i => i.Puzzle.Id == puzzleId)
                        .ToListAsync();

                return images;
            }
        }

        public async Task<IEnumerable<Image>> GetImagesForSession(int sessionId)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var images = await ctx.Images.Where(i => i.SolvingSession.Id == sessionId)
                        .ToListAsync();

                return images;
            }
        }

        private static string GetImagePath(Image image)
        {
            var path = Path.Combine(BASE_DIR, image.Puzzle.Id.ToString());
            if (image.SolvingSession?.Id != null)
            {
                path = Path.Combine(path, image.SolvingSession.Id.ToString());
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, image.FileName);

            return path;
        }

        private static string GetContentType(Image image)
        {
            var extension = Path.GetExtension(image.FileName).TrimStart('.');
            if (extension == "jpg")
            {
                extension = "jpeg";
            }

            return $"image/{extension}";
        }

        private static Image PopulateImageInfo(Image image)
        {
            image.FilePath = GetImagePath(image);
            image.ContentType = GetContentType(image);

            return image;
        }
    }
}
