using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PuzzleTimer.Services
{
    public class SolvingSessionService : ISolvingSessionService
    {
        private readonly ISolvingSessionRepository _sessionRepository;
        private readonly IPuzzleService _puzzleService;

        public SolvingSessionService(ISolvingSessionRepository sessionRepository, IPuzzleService puzzleService)
        {
            _sessionRepository = sessionRepository;
            _puzzleService = puzzleService;
        }

        public async Task<string> CompleteSession(int sessionId)
        {
            var session = await _sessionRepository.GetSolvingSession(sessionId);

            session.Completed = DateTime.Now;

            await _sessionRepository.UpdateSolvingSession(session);
            //TODO: TimeEntry repo complete any running

            if (session.TimeEntries != null && session.TimeEntries.Any())
            {
                var timespans = session.TimeEntries.Where(t => t.EndTime.HasValue).Select(t => t.EndTime.Value.Subtract(t.StartTime)).ToList();

                var totalTime = timespans.Aggregate((curr, next) => curr.Add(next));

                return totalTime.ToString("c");
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

        public async Task<SolvingSession> GetSolvingSession(int id)
        {
            return await _sessionRepository.GetSolvingSession(id);
        }
    }
}
