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
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public TimeEntryRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TimeEntry> Create(int sessionId, int userId, DateTime startTime)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var session = await ctx.SolvingSessions.FirstOrDefaultAsync(s => s.Id == sessionId);
                var user = await ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);

                var timeEntry = new TimeEntry
                {
                    SolvingSession = session,
                    User = user,
                    StartTime = startTime,
                };

                ctx.TimeEntries.Add(timeEntry);

                if (await ctx.SaveChangesAsync() == 1)
                {
                    return timeEntry;
                }
                return null;
            }
        }

        public async Task<TimeEntry> Get(int timeEntryId)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.TimeEntries.FirstOrDefaultAsync(t => t.Id == timeEntryId);
            }
        }

        public async Task<IEnumerable<TimeEntry>> GetAll()
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.TimeEntries.ToListAsync();
            }
        }

        public async Task<IEnumerable<TimeEntry>> GetWhere(Expression<Func<TimeEntry, bool>> predicate)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.TimeEntries.Where(predicate).ToListAsync();
            }
        }

        public async Task<int> Update(TimeEntry entry)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                ctx.Entry(entry).State = EntityState.Modified;
                return await ctx.SaveChangesAsync();
            }
        }
    }
}
