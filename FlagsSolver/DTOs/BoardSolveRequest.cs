using System;
using System.Collections.Generic;

namespace FlagsSolver.DTOs
{
    public class BoardSolverRequest 
    {
        public List<string> Board {get; }
        public int Height { get;}
        public int Width { get;}

        public BoardSolverRequest(List<string> board, int height, int width)
        {
            Board = board;
            Height = height;
            Width = width;
        }

        public override string ToString()
        {
            return String.Format("Height: {0}, Width={1}, Tiles=[{2}]", Height, Width, String.Join(",", Board));
        }

    }
}