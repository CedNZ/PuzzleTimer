using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Repositories
{
    public class PuzzleRepository : IPuzzleRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public PuzzleRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> AddPuzzle(Puzzle puzzle)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                ctx.Puzzles.Add(puzzle);
                return await ctx.SaveChangesAsync();
            }
        }

        public async Task<Puzzle> FindPuzzle(string barcode)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.Puzzles.FirstOrDefaultAsync(p => p.Barcode == barcode);
            }
        }

        public async Task<IEnumerable<Puzzle>> FindPuzzlesByName(string name)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.Puzzles.Where(p => EF.Functions.Like(p.Name, $"{name}%")).ToListAsync();
            }
        }

        public async Task<Puzzle> GetPuzzle(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.Puzzles.FindAsync(id);
            }
        }
    }
}
