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


        public SolvingSessionService(ISolvingSessionRepository sessionRepository, ITimeEntryService timeEntryService, IUserService userService)
        {
            _sessionRepository = sessionRepository;
            _timeEntryService = timeEntryService;
            _userService = userService;
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

                return totalTime.ToString("h'h 'm'm 's's'");
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
                session.TimeTaken = session.TimeEntries.Aggregate(new TimeSpan(), (agg, next) => agg + (next.EndTime.Value - next.StartTime));
            }

            return sessions;
        }

        public async Task<SolvingSession> GetSolvingSession(int id)
        {
            return await _sessionRepository.GetSolvingSession(id);
        }
    }
}
