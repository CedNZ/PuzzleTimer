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
        public async Task<SolvingSession> Get()
        {
            var session = await _solvingSessionService.GetCurrentSession();

            if (session == null)
            {
                throw new ArgumentNullException();
            }
            return session;
        }

        [HttpGet(nameof(CreateSolvingSession), Name = nameof(CreateSolvingSession))]
        public async Task<SolvingSession> CreateSolvingSession([FromQuery]int puzzleId)
        {
            return await _solvingSessionService.CreateSession(puzzleId);
        }
    }
}
