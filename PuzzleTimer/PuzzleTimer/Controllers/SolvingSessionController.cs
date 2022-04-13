using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

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
        public async Task<IActionResult> Get()
        {
            var session = await _solvingSessionService.GetCurrentSession();

            if (session == null)
            {
                return Problem();
            }
            return Ok(session);
        }

        [HttpGet(nameof(GetSessions), Name = nameof(GetSessions))]
        public async Task<IEnumerable<SolvingSession>> GetSessions()
        {
            return await _solvingSessionService.GetSessions();
        }

        [HttpGet(nameof(GetSession), Name = nameof(GetSession))]
        public async Task<SolvingSession> GetSession([FromQuery] int sessionId)
        {
            return await _solvingSessionService.GetSolvingSession(sessionId);
        }

        [HttpGet(nameof(GetSessionTime), Name = nameof(GetSessionTime))]
        public async Task<string> GetSessionTime([FromQuery] int sessionId)
        {
            return await _solvingSessionService.GetSessionTime(sessionId);
        }

        [HttpGet(nameof(CreateSolvingSession), Name = nameof(CreateSolvingSession))]
        public async Task<SolvingSession> CreateSolvingSession([FromQuery] int puzzleId)
        {
            return await _solvingSessionService.CreateSession(puzzleId);
        }

        [HttpGet(nameof(CompleteSession), Name = nameof(CompleteSession))]
        public async Task<string> CompleteSession([FromQuery] int sessionId)
        {
            return await _solvingSessionService.CompleteSession(sessionId);
        }

        [HttpGet(nameof(AddUser), Name = nameof(AddUser))]
        public async Task<SolvingSession> AddUser([FromQuery] int sessionId, [FromQuery] int userId)
        {
            return await _solvingSessionService.AddUser(sessionId, userId);
        }
    }
}
