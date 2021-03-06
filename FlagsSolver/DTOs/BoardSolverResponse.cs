using System;
using System.Collections.Generic;

namespace FlagsSolver.DTOs
{
    public class BoardSolverResponse 
    {
        public List<Coordinate> Flags {get; set;}
        public List<Coordinate> NonFlags {get; set;}

        public BoardSolverResponse(List<Coordinate> flags, List<Coordinate> nonFlags)
        {
            Flags = flags;
            NonFlags = nonFlags;
        }
    }

    public class Coordinate
    {
        public int Y {get; set;}        
        public int X {get; set;}

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


}