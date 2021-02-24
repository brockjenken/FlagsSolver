using System.Net;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FlagsSolver.DTOs;
using FlagsSolver.Models;
using FlagsSolver.Service;
using FlagsSolver.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlagsSolver.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SolverController : ControllerBase
    {
        private readonly ILogger<SolverController> _logger;

        public SolverController(ILogger<SolverController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<string> Solve(BoardSolverRequest request)
        {
            _logger.LogInformation(String.Format("Solver request with: {0}", request));

            Board board = new Board(
                request.Height,
                request.Width,
                TileUtil.DeserialzeTiles(request.Board, request.Height, request.Width));

            Board solvedBoard = BoardSolver.SolveBoard(board);
            
            return TileUtil.SerialzeTiles(solvedBoard.Tiles);
        }
    }
}
