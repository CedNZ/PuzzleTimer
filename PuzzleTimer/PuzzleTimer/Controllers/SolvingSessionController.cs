﻿using Microsoft.AspNetCore.Mvc;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System;
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
