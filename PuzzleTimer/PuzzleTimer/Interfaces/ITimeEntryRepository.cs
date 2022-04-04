using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface ITimeEntryRepository
    {
        public Task<TimeEntry> Create(int sessionId, int userId, DateTime startTime);

        public Task<TimeEntry> Get(int timeEntryId);

        public Task<IEnumerable<TimeEntry>> GetAll();

        public Task<IEnumerable<TimeEntry>> GetWhere(Expression<Func<TimeEntry, bool>> predicate);

        public Task<int> Update(TimeEntry entry);

    }
}
