using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task<int> AddPuzzle(Puzzle puzzle)
        {
            throw new NotImplementedException();
        }

        public Task<Puzzle> FindPuzzle(string barcode)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return ctx.Puzzles.FirstOrDefaultAsync(p => p.Barcode == barcode);
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
