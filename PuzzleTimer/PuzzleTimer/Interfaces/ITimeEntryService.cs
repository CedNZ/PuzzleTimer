using System;
using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface ITimeEntryService
    {
        public Task<TimeEntry> StartNew(int sessionId, int userId, DateTime startTime);
        public Task<TimeEntry> Stop(int timeEntryId, DateTime stopTime);
        public Task<TimeEntry> GetCurrent(int sessionId, int userId);
        public Task<TimeSpan> GetTotalTime(int sessionId, int userId);
        public Task<TimeEntry> AddComment(int timeEntryId, string comment);
    }
}
