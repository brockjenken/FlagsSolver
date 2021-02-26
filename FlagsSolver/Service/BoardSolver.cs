using System.Collections.Immutable;
using System.Linq;
using System.Collections.Generic;
using FlagsSolver.Models;

namespace FlagsSolver.Service
{
    public class BoardSolver
    {

        /// <summary>This method will iteratively look through each tile and check to see if it can 
        /// solve the surrounding tiles. If a change is detected, the method will loop again and re-evaluate.
        ///  While the board is solved by reference, it is also returned for stylistic convenience.
        /// </summary>
        /// <param name="board">Board to be solved</param>
        /// <returns>The solved board of type <typeparamref name="Board"></returns>
        public static Board SolveBoard(Board board)
        {
            List<Tile> scanTiles = new List<Tile>(board.Tiles)
                .Where(t => t.Type == TileType.NUMBER)
                .ToList();

            bool boardChanged = true;
            while (boardChanged)
            {
                boardChanged = false;
                List<Tile> newScanTiles = new List<Tile>();

                scanTiles.ForEach(t => {
                    bool tileChange = SolveAdjacent(board, t);
                    boardChanged = boardChanged || tileChange;
                    if (!tileChange)
                    {
                        newScanTiles.Add(t);
                    }
                });

                scanTiles = newScanTiles;
            }

            return board;
        }

        private static bool SolveAdjacent(Board board, Tile tile)
        {
            if (tile.Type != TileType.NUMBER)
            {
                return false;
            }

            ImmutableList<Tile> adjacentTiles = board.GetAdjacentTiles(tile.X, tile.Y);

            int tileCount = tile.Value ?? default(int);
            int flagCount = adjacentTiles.AsEnumerable().Count( t => t.Type == TileType.FLAG);
            int remainingFlagCount = tileCount - flagCount;
            int unrevealedTileCount = adjacentTiles.AsEnumerable().Count(t => t.Type == TileType.UNKNOWN);

            bool boardChanged = false;

            if (remainingFlagCount == 0)
            {
                boardChanged = true;
                adjacentTiles.ForEach(t => {
                    if (t.Type == TileType.UNKNOWN) {
                        t.Type = TileType.NUMBER;
                        t.Solved = true;
                }});
            }
            else if (remainingFlagCount > 0 && remainingFlagCount == unrevealedTileCount)
            {
                boardChanged = true;
                adjacentTiles.ForEach(t => {
                    if (t.Type == TileType.UNKNOWN) {
                        t.Type = TileType.FLAG;
                        t.Solved = true;
                }});
            }

            return boardChanged;
        }
    }
}
