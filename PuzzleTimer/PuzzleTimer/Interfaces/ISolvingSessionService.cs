using PuzzleTimer.Models;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface ISolvingSessionService
    {
        public Task<SolvingSession> CreateSession(int puzzleId);

        public Task<SolvingSession> GetSolvingSession(int id);

        public Task<int> DeleteSolvingSession(SolvingSession solvingSession);

        public Task<SolvingSession> GetCurrentSession();

        public Task<string> CompleteSession(int sessionId);

        public Task<SolvingSession> AddUser(int sessionId, int userId);
    }
}
