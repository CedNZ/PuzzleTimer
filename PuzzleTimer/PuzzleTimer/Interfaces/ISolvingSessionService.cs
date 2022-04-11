using System.Collections.Generic;
using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface ISolvingSessionService
    {
        public Task<SolvingSession> CreateSession(int puzzleId);

        public Task<SolvingSession> GetSolvingSession(int id);

        public Task<IEnumerable<SolvingSession>> GetSessions();

        public Task<int> DeleteSolvingSession(SolvingSession solvingSession);

        public Task<SolvingSession> GetCurrentSession();

        public Task<string> GetSessionTime(int sessionId);

        public Task<string> CompleteSession(int sessionId);

        public Task<SolvingSession> AddUser(int sessionId, int userId);
    }
}
