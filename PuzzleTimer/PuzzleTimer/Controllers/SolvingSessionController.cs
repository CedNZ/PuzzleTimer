using Microsoft.AspNetCore.Mvc;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleTimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolvingSessionController : ControllerBase
    {
        private readonly ISolvingSessionService _solvingSessionService;

        public SolvingSessionController(ISolvingSessionService solvingSessionService)
        {
            _solvingSessionService = solvingSessionService;
        }

        [HttpGet]
        public SolvingSession Get()
        {
            return new SolvingSession
            {
                Id = 1,
                Started = DateTime.Now,
            };
        }

        [HttpPost]
        public async Task<SolvingSession> CreateSolvingSession(int puzzleId)
        {
            return await _solvingSessionService.CreateSession(puzzleId);
        }
    }
}
