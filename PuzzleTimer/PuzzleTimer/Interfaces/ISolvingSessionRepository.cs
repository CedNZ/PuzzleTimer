using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface ISolvingSessionRepository
    {
        public Task<SolvingSession> GetSolvingSession(int id);

        public Task<int> AddSolvingSession(SolvingSession solvingSession, int puzzleId);

        public Task<int> UpdateSolvingSession(SolvingSession solvingSession);

        public Task<int> DeleteSolvingSession(SolvingSession solvingSession);

        public Task<IEnumerable<SolvingSession>> GetAllSolvingSessions();

        public Task<IEnumerable<SolvingSession>> GetSolvingSessionsWhere(Expression<Func<SolvingSession, bool>> predicate);

        public Task<SolvingSession> AddUser(int sessionId, int userId);
    }
}
