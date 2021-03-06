using System.ComponentModel;
using System;
using System.Collections.Generic;
using FlagsSolver.DTOs;
using FlagsSolver.Models;
using FlagsSolver.Service;
using FlagsSolver.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlagsSolver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolverController : ControllerBase
    {
        private static string SOLVE_EVENT = "solveEvent";
        private readonly ILogger<SolverController> _logger;

        public SolverController(ILogger<SolverController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public BoardSolverResponse Solve(BoardSolverRequest request)
        {
            _logger.LogInformation(String.Format("event={0}; status=started; request={1}"),SOLVE_EVENT, request);

            Board board = new Board(
                request.Height,
                request.Width,
                TileUtil.DeserialzeTiles(request.Board, request.Height, request.Width));

            Board solvedBoard = BoardSolver.SolveBoard(board);

            Tuple<List<Coordinate>, List<Coordinate>> results = TileUtil.GetCoordinatesOfChanges(board, solvedBoard);
            
            _logger.LogInformation(String.Format("event={0}; status=success; request={1}"),SOLVE_EVENT, request);

            return new BoardSolverResponse(results.Item1, results.Item2);
        }
    }
}
