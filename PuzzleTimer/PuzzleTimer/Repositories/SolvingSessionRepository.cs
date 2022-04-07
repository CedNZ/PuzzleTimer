using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<SolvingSession> AddUser(int sessionId, int userId)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var solvingSession = await ctx.SolvingSessions
                                        .Include(s => s.Puzzle)
                                        .Include(s => s.Users)
                                        .FirstOrDefaultAsync(s => s.Id == sessionId);

                var user = await ctx.Users.FindAsync(userId);

                solvingSession.Users.Add(user);

                var lines = await ctx.SaveChangesAsync();
                if (lines == 1)
                {
                    return solvingSession;
                }
                return null;
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
                return await ctx.SolvingSessions
                    .Include(s => s.Users)
                    .Include(s => s.Puzzle)
                    .Include(s => s.TimeEntries)
                    .ToListAsync();
            }
        }

        public async Task<SolvingSession> GetSolvingSession(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.SolvingSessions
                    .Include(s => s.Puzzle)
                    .Include(s => s.Users)
                    .Include(s => s.TimeEntries)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
        }

        public async Task<IEnumerable<SolvingSession>> GetSolvingSessionsWhere(Expression<Func<SolvingSession, bool>> predicate)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.SolvingSessions
                    .Include(s => s.Puzzle)
                    .Include(s => s.Users)
                    .Where(predicate)
                    .ToListAsync();
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
