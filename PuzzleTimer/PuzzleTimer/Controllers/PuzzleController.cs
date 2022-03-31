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
            var puzzle = await _puzzleService.FindPuzzleInfo(barcode);

            if (puzzle == null)
            {
                throw new Exception();
            }
            return puzzle;
        }

        [HttpPost("CreatePuzzle", Name = "CreatePuzzle")]
        public Task<Puzzle> CreatePuzzle(string barcode, string name, int pieceCount)
        {
            return _puzzleService.CreatePuzzle(barcode, name, pieceCount);
        }
    }
}
