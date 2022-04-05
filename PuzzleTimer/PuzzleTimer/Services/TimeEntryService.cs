using System;
using System.Linq;
using System.Threading.Tasks;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly ITimeEntryRepository _repository;

        public TimeEntryService(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeEntry> AddComment(int timeEntryId, string comment)
        {
            var timeEntry = await _repository.Get(timeEntryId);
            timeEntry.Comment = comment;

            if (await _repository.Update(timeEntry) == 1)
            {
                return timeEntry;
            }
            return null;
        }

        public async Task<TimeEntry> GetCurrent(int sessionId, int userId)
        {
            var entries = await _repository.GetWhere(te => te.SolvingSession.Id == sessionId && te.User.Id == userId && te.EndTime == null);
            return entries.FirstOrDefault();
        }

        public async Task<TimeSpan> GetTotalTime(int sessionId, int userId)
        {
            var entries = await _repository.GetWhere(te => te.SolvingSession.Id == sessionId && te.User.Id == userId && te.EndTime != null);

            return entries.Aggregate(new TimeSpan(), (agg, next) => agg + (next.EndTime.Value - next.StartTime));
        }

        public async Task<TimeEntry> StartNew(int sessionId, int userId, DateTime startTime)
        {
            return await _repository.Create(sessionId, userId, startTime);
        }

        public async Task<TimeEntry> Stop(int timeEntryId, DateTime stopTime)
        {
            var timeEntry = await _repository.Get(timeEntryId);
            timeEntry.EndTime = stopTime;

            if (await _repository.Update(timeEntry) == 1)
            {
                return timeEntry;
            }
            return null;
        }
    }
}
