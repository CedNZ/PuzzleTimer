using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetPuzzle([FromQuery] string barcode)
        {
            var puzzle = await _puzzleService.FindPuzzleInfo(barcode);

            if (puzzle == null)
            {
                return Problem();
            }
            return Ok(puzzle);
        }


        [HttpGet(nameof(FindPuzzlesByName), Name = nameof(FindPuzzlesByName))]
        public async Task<IEnumerable<Puzzle>> FindPuzzlesByName([FromQuery] string name)
        {
            return await _puzzleService.FindPuzzlesByName(name);
        }

        [HttpPost("CreatePuzzle", Name = "CreatePuzzle")]
        public async Task<Puzzle> CreatePuzzle([FromBody] PuzzleDTO request)
        {
            return await _puzzleService.CreatePuzzle(request.Barcode, request.Name, request.PieceCount);
        }
    }

    public class PuzzleDTO
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int PieceCount { get; set; }
    }
}
