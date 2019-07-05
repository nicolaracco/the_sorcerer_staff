using System;
using UnityEngine;

namespace Sorcerer.Map
{
    public enum Direction
    {
        N, NE, E, SE, S, SW, W, NW
    }

    public static class DirectionMethods
    {
        public static Direction ToSorcererDirection(this Vector2Int self)
        {
            if (self.x == 0)
            {
                if (self.y == -1)
                    return Direction.S;
                if (self.y == 1)
                    return Direction.N;
            } 
            else if (self.x == 1)
            {
                if (self.y == -1)
                    return Direction.SE;
                if (self.y == 1)
                    return Direction.NE;
                if (self.y == 0)
                    return Direction.E;
            }
            else if (self.x == -1)
            {
                if (self.y == -1)
                    return Direction.SW;
                if (self.y == 1)
                    return Direction.NW;
                if (self.y == 0)
                    return Direction.W;
            }
            throw new Exception("Vector2Int out of range and cannot be cast to direction");
        }
    }
}