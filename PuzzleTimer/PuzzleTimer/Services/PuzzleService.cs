﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Services
{
    public class PuzzleService : IPuzzleService
    {
        private readonly IPuzzleRepository _puzzleRepository;

        public PuzzleService(IPuzzleRepository puzzleRepository)
        {
            _puzzleRepository = puzzleRepository;
        }

        public async Task<Puzzle> GetPuzzle(int id)
        {
            var puzzle = await _puzzleRepository.GetPuzzle(id);

            return puzzle;
        }

        public async Task<Puzzle> FindPuzzleInfo(string barcode)
        {
            var puzzle = await _puzzleRepository.FindPuzzle(barcode);

            return puzzle;
        }

        public async Task<Puzzle> CreatePuzzle(string barcode, string name, int pieceCount)
        {
            var puzzle = new Puzzle
            {
                Name = name,
                Barcode = barcode,
                PieceCount = pieceCount,
            };

            var saveResult = await _puzzleRepository.AddPuzzle(puzzle);

            if (saveResult == 1)
            {
                return puzzle;
            }
            return null;
        }

        public async Task<IEnumerable<Puzzle>> FindPuzzlesByName(string name)
        {
            if (name is not null && name.Length > 0)
            {
                return await _puzzleRepository.FindPuzzlesByName(name);
            }
            return default;
        }
    }
}
