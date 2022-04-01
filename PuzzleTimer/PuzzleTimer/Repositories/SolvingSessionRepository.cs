using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Repositories
{
    public class SolvingSessionRepository : ISolvingSessionRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public SolvingSessionRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> AddSolvingSession(SolvingSession solvingSession, int puzzleId)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var puzzle = ctx.Puzzles.Find(puzzleId);
                solvingSession.Puzzle = puzzle;

                ctx.SolvingSessions.Add(solvingSession);
                return await ctx.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteSolvingSession(SolvingSession solvingSession)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                ctx.SolvingSessions.Remove(solvingSession);
                return await ctx.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SolvingSession>> GetAllSolvingSessions()
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.SolvingSessions.ToListAsync();
            }
        }

        public async Task<SolvingSession> GetSolvingSession(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.SolvingSessions.FindAsync(id);
            }
        }

        public async Task<IEnumerable<SolvingSession>> GetSolvingSessionsWhere(Expression<Func<SolvingSession, bool>> predicate)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.SolvingSessions.Where(predicate).ToListAsync();
            }
        }

        public async Task<int> UpdateSolvingSession(SolvingSession solvingSession)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                ctx.Entry(solvingSession).Property(p => p.Completed).IsModified = true;
                return await ctx.SaveChangesAsync();
            }
        }
    }
}
