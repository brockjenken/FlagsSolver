using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FlagsSolver.Models
{
    public class Board
    {
        public ImmutableList<Tile> Tiles { get; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Board(int height, int width, List<Tile> tiles)
        {
            if (height <= 0 || width <= 0)
            {
                throw new ArgumentException("Height and width must be positive integers.");
            }

            Height = height;
            Width = width;
            Tiles = CheckTiles(tiles);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x > Height - 1 || y < 0 || y > Width)
            {
                return null;
            }

            return Tiles[x * Width + y];
        }

        public ImmutableList<Tile> GetAdjacentTiles(int x, int y)
        {
            List<Tuple<int, int>> coordinates = Enumerable.Range(x-1, 3)
                .Select(x => Enumerable.Range(y-1, 3)
                    .Select(y => new Tuple<int, int>(x, y)))
                .SelectMany(i => i)
                .ToList();
            
            return coordinates.Select(c => GetTile(c.Item1, c.Item2)).ToImmutableList();
        }

        private ImmutableList<Tile> CheckTiles(List<Tile> tiles)
        {
            if (Width <= 0 || Height <= 0)
            {
                throw new ArgumentNullException("Height or Width not initialized properly.");
            }

            if (tiles.Count != Height * Width)
            {
                throw new ArgumentException("Number of tiles must match the size of the board.");
            }

            return tiles.ToImmutableList();
        }
    }
}


// {"board" : ["null, "null", ..., "1", "3", "A", ..., "0", "B"], "N": 48, "M": 48}

// null 0 1 2 3 4 5 6 7 8 A B
