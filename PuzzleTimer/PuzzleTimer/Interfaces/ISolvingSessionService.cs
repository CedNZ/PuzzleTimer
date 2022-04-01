using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface ISolvingSessionService
    {
        public Task<SolvingSession> CreateSession(int puzzleId);

        public Task<SolvingSession> GetSolvingSession(int id);

        public Task<int> DeleteSolvingSession(SolvingSession solvingSession);

        public Task<SolvingSession> GetCurrentSession();
    }
}
