using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FlagsSolver.Models;

namespace FlagsSolver.Util
{
    public class TileUtil
    {
        private static readonly Dictionary<string, Func<int, int, Tile>> DESERIALIZE_MAP = new Dictionary<string, Func<int, int, Tile>>
            {
                { "A", (int x, int y) => new Tile(TileType.FLAG, null, true, x, y)},
                { "B", (int x, int y) => new Tile(TileType.FLAG, null,  true, x, y)},
                { "null", (int x, int y) => new Tile(TileType.UNKNOWN, null, false, x, y)},
                { "0", (int x, int y) => new Tile(TileType.NUMBER, 0, true, x, y)},
                { "1", (int x, int y) => new Tile(TileType.NUMBER, 1, true, x, y)},
                { "2", (int x, int y) => new Tile(TileType.NUMBER, 2, true, x, y)},
                { "3", (int x, int y) => new Tile(TileType.NUMBER, 3, true, x, y)},
                { "4", (int x, int y) => new Tile(TileType.NUMBER, 4, true, x, y)},
                { "5", (int x, int y) => new Tile(TileType.NUMBER, 5, true, x, y)},
                { "6", (int x, int y) => new Tile(TileType.NUMBER, 6, true, x, y)},
                { "7", (int x, int y) => new Tile(TileType.NUMBER, 7, true, x, y)},
                { "8", (int x, int y) => new Tile(TileType.NUMBER, 8, true, x, y)},
            };

        public static List<Tile> DeserialzeTiles(List<string> rawTiles, int height, int width)
        {
            List<Tile> deserializedTiles = new List<Tile>();
            for (int i = 0; i < rawTiles.Count; i++)
            {
                int x = i % width;
                int y = (int) Math.Floor((decimal) i / height);
                deserializedTiles.Add(TileUtil.DeserializeTile(rawTiles[i], x, y));
            }

            return deserializedTiles;
        }

        public static List<string> SerialzeTiles(ImmutableList<Tile> tiles)
        {
            List<string> serializedTiles = new List<string>();
            tiles.ForEach(s =>
            {
                serializedTiles.Add(TileUtil.SerializeTile(s));
            });

            return serializedTiles;
        }

        private static Tile DeserializeTile(string tileString, int x, int y)
        {
            if (tileString == null){
                tileString = "null";
            }

            if (!DESERIALIZE_MAP.ContainsKey(tileString))
            {
                throw new ArgumentException(string.Format("{0} is not a valid tile input", tileString));
            }

            return DESERIALIZE_MAP[tileString](x, y);
        }

        private static string SerializeTile(Tile tile)
        {
            return tile.Value == null ? "null" : tile.Value.GetValueOrDefault().ToString();
        }
    }
}
