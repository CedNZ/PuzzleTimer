using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PuzzleController : ControllerBase
    {
        private readonly IPuzzleService _puzzleService;

        public PuzzleController(IPuzzleService puzzleService)
        {
            _puzzleService = puzzleService;
        }

        [HttpGet("GetPuzzle", Name = "GetPuzzle")]
        public async Task<Puzzle> GetPuzzle([FromQuery]string barcode)
        {
            return await _puzzleService.FindPuzzleInfo(barcode);
        }
    }
}
