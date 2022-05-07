using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Services
{
    public class SolvingSessionService : ISolvingSessionService
    {
        private readonly ISolvingSessionRepository _sessionRepository;
        private readonly ITimeEntryService _timeEntryService;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        private const string TIMESPAN_TEMPLATE = "'PT'h'H'm'M's'S'";

        public SolvingSessionService(ISolvingSessionRepository sessionRepository, ITimeEntryService timeEntryService, IUserService userService, IImageService imageService)
        {
            _sessionRepository = sessionRepository;
            _timeEntryService = timeEntryService;
            _userService = userService;
            _imageService = imageService;
        }

        public async Task<SolvingSession> AddUser(int sessionId, int userId)
        {
            return await _sessionRepository.AddUser(sessionId, userId);
        }

        public async Task<string> CompleteSession(int sessionId)
        {
            var stopTime = DateTime.Now;

            var session = await _sessionRepository.GetSolvingSession(sessionId);

            session.Completed = stopTime;

            await _sessionRepository.UpdateSolvingSession(session);
            var users = await _userService.GetUsersForSession(sessionId);

            foreach (var user in users)
            {
                var timeEntry = await _timeEntryService.GetCurrent(sessionId, user.Id);
                if (timeEntry != null)
                {
                    await _timeEntryService.Stop(timeEntry.Id, stopTime);
                }
            }

            if (session.TimeEntries != null && session.TimeEntries.Any())
            {
                var totalTime = session.TimeEntries
                    .Where(t => t.EndTime.HasValue)
                    .Aggregate(new TimeSpan(), (agg, next) => agg + (next.EndTime.Value - next.StartTime));

                return totalTime.ToString(TIMESPAN_TEMPLATE);
            }
            return "";
        }

        public async Task<SolvingSession> CreateSession(int puzzleId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var newSession = new SolvingSession
                {
                    Started = DateTime.Now,
                };

                var rows = await _sessionRepository.AddSolvingSession(newSession, puzzleId);

                if (rows == 1)
                {
                    scope.Complete();
                }

                return newSession;
            }
        }

        public async Task<int> DeleteSolvingSession(SolvingSession solvingSession)
        {
            return await _sessionRepository.DeleteSolvingSession(solvingSession);
        }

        public async Task<SolvingSession> GetCurrentSession()
        {
            Expression<Func<SolvingSession, bool>> predicate = s => s.Completed == null;

            var sessions = await _sessionRepository.GetSolvingSessionsWhere(predicate);

            return sessions.FirstOrDefault();
        }

        public async Task<IEnumerable<SolvingSession>> GetSessions()
        {
            var sessions = await _sessionRepository.GetAllSolvingSessions();

            foreach (var session in sessions)
            {
                session.TimeTaken = session.TimeEntries
                    .Aggregate(new TimeSpan(), (agg, next) => agg + ((next.EndTime ?? DateTime.Now) - next.StartTime))
                    .ToString(TIMESPAN_TEMPLATE);

                session.TimeEntries = null;

                session.Image = (await _imageService.GetImagesForPuzzle(session.Puzzle.Id)).FirstOrDefault();
            }

            return sessions;
        }

        public async Task<string> GetSessionTime(int sessionId)
        {
            var timeSpan = await _timeEntryService.GetTotalTimeForSession(sessionId);

            return timeSpan.ToString(TIMESPAN_TEMPLATE);
        }

        public async Task<SolvingSession> GetSolvingSession(int id)
        {
            return await _sessionRepository.GetSolvingSession(id);
        }
    }
}
