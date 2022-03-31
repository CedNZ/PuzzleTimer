using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface ISolvingSessionRepository
    {
        public Task<SolvingSession> GetSolvingSession(int id);

        public Task<int> AddSolvingSession(SolvingSession solvingSession);

        public Task<int> DeleteSolvingSession(SolvingSession solvingSession);
    }
}
