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

        public string toString()
        {
            return String.Format("Type={0}, Value={1}, Solved={2}, X={3}, Y={4}", Type, Value, Solved, X, Y);
        }
    }
}
