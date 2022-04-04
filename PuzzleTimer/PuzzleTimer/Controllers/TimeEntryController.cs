using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntryController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        [HttpGet(nameof(Start), Name = nameof(Start))]
        public async Task<TimeEntry> Start([FromQuery] int sessionId, [FromQuery] int userId)
        {
            return await _timeEntryService.StartNew(sessionId, userId, DateTime.Now);
        }

        [HttpGet(nameof(Stop), Name = nameof(Stop))]
        public async Task<TimeEntry> Stop([FromQuery] int timeEntryId)
        {
            return await _timeEntryService.Stop(timeEntryId, DateTime.Now);
        }

        [HttpGet(nameof(GetCurrent), Name = nameof(GetCurrent))]
        public async Task<TimeEntry> GetCurrent([FromQuery] int sessionId, [FromQuery] int userId)
        {
            return await _timeEntryService.GetCurrent(sessionId, userId);
        }

        [HttpPost(nameof(AddComment), Name = nameof(AddComment))]
        public async Task<TimeEntry> AddComment([FromQuery] int timeEntryId, [FromBody] string comment)
        {
            return await _timeEntryService.AddComment(timeEntryId, comment);
        }

        [HttpGet(nameof(GetTotalTime), Name = nameof(GetTotalTime))]
        public async Task<string> GetTotalTime([FromQuery] int sessionId, [FromQuery] int userId)
        {
            var totalTime = await _timeEntryService.GetTotalTime(sessionId, userId);
            return totalTime.ToString("h'h 'm'm 's's'");
        }
    }
}
