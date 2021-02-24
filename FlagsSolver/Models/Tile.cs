using System;
namespace FlagsSolver.Models
{
    public class Tile
    {
        public TileType Type { get; set;}
        public int? Value { get; set;}
        public bool Solved { get; set;}

        public int X {get;}
        public int Y {get;}
        public Tile(TileType type, Nullable<int> value, bool solved, int x, int y)
        {
            Type = type;
            Value = value;
            Solved = solved;
            X = x;
            Y = y;
        }
    }
}
