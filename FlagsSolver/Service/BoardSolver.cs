using System.Collections.Immutable;
using System.Linq;
using System.Collections.Generic;
using FlagsSolver.Models;

namespace FlagsSolver.Service
{
    public class BoardSolver
    {
        public static Board SolveBoard(Board board)
        {
            // See Pseudocode below

            return board;
        }


        // private static int ScanBoardForFlags(Board board)
        // {
        //     Enumerable.Range(0, board.Width).ToList().ForEach(x => {
        //         Enumerable.Range(0, board.Height).ToList().ForEach( y => {
        //             FindAdjacentFlags(board, board.GetTile(x, y), x, y);
        //         });
        //     });
        // }

        private static int FindAdjacentFlags(Board board, Tile tile, int x, int y)
        {
            if (tile.Type != TileType.NUMBER)
            {
                return 0;
            }

            ImmutableList<Tile> adjacentTiles = board.GetAdjacentTiles(x, y);

            int tileCount = tile.Value ?? default(int);
            int flagCount = adjacentTiles.AsEnumerable().Count( t => t.Type == TileType.FLAG);
            int remainingFlagCount = tileCount - flagCount;
            int unrevealedTileCount = adjacentTiles.AsEnumerable().Count(t => t.Type == TileType.UNKNOWN);

            int numberFound = 0;

            if (remainingFlagCount > 0 && remainingFlagCount == unrevealedTileCount)
            {
                numberFound = remainingFlagCount;
            }

            adjacentTiles.ForEach(t => {
                t.Type = TileType.FLAG;
                t.Solved = true;
            });

            return numberFound;
        }
    }
}

//  Pseudocode
//
//  Each unrevealed tile can be assigned a probability for being a flag.
//  But we're not there yet.
//
//  First implement solvers for unrevealed tiles that are
//  (1) guaranteed to be flags
//  (2) guaranteed to be not-flags (i.e. numbers)
//  
//
//  class Tile:
//      enum  TileType         [UNKNOWN,NUMBER,FLAG]
//      int   TileNumValue     [0,1,2,3,4,5,6,7,8]
//      bool  TileSolved       [true,false]
//
//  Example board input: (n = null)
//
//      n n n n n n n n
//      A n n n n 2 n n
//      B 4 n n n 2 2 2
//      n n n n n 2 0 0
//      n n n n n 2 2 1
//      A 1 n n n n n B
//
//  The solver doesn't need to know who found which flags, just which tiles are flags.
//
//  Deserialized board:
//
//      TileType:           TileNumValue:       TileSolved: [- false, * true]
//
//      U U U U U U U U     0 0 0 0 0 0 0 0     - - - - - - - -
//      F U U U U N U U     0 0 0 0 0 2 0 0     - - - - - - - -
//      F N U U U N N N     0 4 0 0 0 2 2 2     - - - - - - - -
//      U U U U U N N N     0 0 0 0 0 2 0 0     - - - - - - - -
//      U U U U U N N N     0 0 0 0 0 2 2 1     - - - - - - - -
//      F N U U U U U F     0 1 0 0 0 0 0 0     - - - - - - - -
//
//  After solving:
//
//      TileType:           TileNumValue:       TileSolved:
//      (changed)           (unchanged)         (changed)
//
//      U U U U U U U U     0 0 0 0 0 0 0 0     - - - - - - - -
//      F U U U U N F F     0 0 0 0 0 2 0 0     - - - - - - * *
//      F N U U U N N N     0 4 0 0 0 2 2 2     - - - - - - - -
//      U U U U U N N N     0 0 0 0 0 2 0 0     - - - - - - - -
//      U U U U U N N N     0 0 0 0 0 2 2 1     - - - - - - - -
//      F N U U U U U F     0 1 0 0 0 0 0 0     - - - - - - - -
//
//  Return to front end:
//
//      {
//          "flags"    : [{1,6},{1,7}],
//          "notflags" : []
//      }
//
//
//  board.tile(i,j):
//      // Helper function
//      // Return (reference to tile i,j) or (null) if i,j out of range
//      return (i >= 0 && j >= 0 && i < N && j < M) ? tiles[i * M + j] : null;
//
//  board.adjacent(i,j):
//      // Helper function
//      // Return the tiles adjacent to tile(i,j)
//      //
//      // Top/Centre/Bottom - Left/Centre/Right
//      //
//      //   T      TL TC TR
//      // L C R    CL CC CR
//      //   B      BL BC BR
//      //
//      // Here CC is tile i,j
//
//      const TL = board.tile(i - 1, j - 1);
//      const TC = board.tile(i - 1, j - 0);
//      const TR = board.tile(i - 1, j + 1);
//      const CL = board.tile(i - 0, j - 1);
//      const CR = board.tile(i - 0, j + 1);
//      const BL = board.tile(i + 1, j - 1);
//      const BC = board.tile(i + 1, j - 0);
//      const BR = board.tile(i + 1, j + 1);
//
//      // These should be references not copies
//      // Some of these may be nulls
//      return [TL, TC, TR, CL, CR, BL, BC, BR];
//
//  find_flags:
//      // Find and label unrevealed tiles that are guaranteed to be flags
//      // A single call to this function may not be enough
//      // This function should be called iteratively until it returns zero
//      //
//      // Logic:
//      // Given a number tile (N = 0..8) and the status of the tiles adjacent to it,
//      // we know:
//      //  N - num. flags total around this tile
//      //  F - num. flags already revealed around this tile (revealed by player OR by a previous iteration of find_flags)
//      //  G - num. flags remaining hidden around this tile (excluding those labelled as flag/not-flag by scanner)
//      //  H - num. unrevealed tiles around this tile
//      //
//      //  if (G > 0 && G == H):
//      //    all hidden tiles are flags
//      
//      nfound = 0;
//
//      // Get all the revealed number tiles
//      numberTiles = board.getNumberTiles()
//
//      // Iterate over all the revealed numeric tiles (values 0-8)
//      for each tile in numberTiles:
//
//          // Array of adjacent tiles
//          const adjacent = board.adjacent(tile.i, tile.j);
//
//          const numAdjacentFlags = {get number of adjacent flags}
//          const numAdjacentHidden = {get number of adjacent unrevealed flags}
//          const numRemainingFlags = {get number of remaining flags}
//
//          if (numRemainingFlags > 0 && numRemainingFlags == numAdjacentHidden)
//              nfound = numRemainingFlags
//              label all adjacent unrevealed tiles as hidden flags
//
//      // Caller needs to know how many flags were found.
//      // If not zero, then find_flags should be called again.
//      return nfound;
//
//
//  find_notflags():
//      // Find and label unrevealed tiles that are guaranteed to not be flags
//      // Similar logic to find_flags
//
//      nfound = 0;
//
//      numberTiles = board.getNumberTiles()
//
//      for each tile in numberTiles:
//
//          const adjacent = board.adjacent(tile.i, tile.j);
//
//          const N = {this tile's numeric value}
//          const numAdjacentFlags = {get number of adjacent flags}
//          const numRemainingFlags = N - numAdjacentFlags
//          const numAdjacentHidden = {get number of adjacent hidden flags}
//
//          if (numRemainingFlags == 0):
//              nfound = numAdjacentHidden
//              label all unrevealed/unlabeled flags around this tile as non-flags
//
//          return nfound;
