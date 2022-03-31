using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<SolvingSession> CreateSession(int puzzleId)
        {
            var puzzle = await _puzzleService.GetPuzzle(puzzleId);

            if (puzzle == null)
            {
                throw new ArgumentNullException();
            }

            var newSession = new SolvingSession
            {
                Started = DateTime.Now,
                Puzzle = puzzle,
            };

            await _sessionRepository.AddSolvingSession(newSession);

            return newSession;
        }

        public async Task<int> DeleteSolvingSession(SolvingSession solvingSession)
        {
            return await _sessionRepository.DeleteSolvingSession(solvingSession);
        }

        public async Task<SolvingSession> GetSolvingSession(int id)
        {
            return await _sessionRepository.GetSolvingSession(id);
        }
    }
}
