using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sorcerer.Map
{
    public static class MapNavigationUtils
    {
        /// <summary>
        /// Get an IEnumerable of outermost border Cells in a square area around the center Cell up to the specified distance
        /// Taken from: https://github.com/FaronBracy/RogueSharp/blob/master/RogueSharp/Map.cs
        /// </summary>
        /// <param name="center">center Cell</param>
        /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
        /// <returns>IEnumerable of outermost border Cells in a square area around the center Cell</returns>
        public static IEnumerable<ICell> BorderCellsInSquare(ICellContainer map, Vector2Int center, int distance)
        {
            Vector2Int min = new Vector2Int(
                Math.Max(0, center.x - distance),
                Math.Max(0, center.y - distance)
            );
            Vector2Int max = new Vector2Int(
                Math.Min(map.Width - 1, center.x + distance),
                Math.Min(map.Height - 1, center.y + distance)
            );
            List<ICell> borderCells = new List<ICell>();

            for (int x = min.x; x <= max.x; x++)
            {
                borderCells.Add(map.CellAt(x, min.y));
                borderCells.Add(map.CellAt(x, max.y));
            }
            for (int y = min.y + 1; y <= max.y - 1; y++)
            {
                borderCells.Add(map.CellAt(min.x, y));
                borderCells.Add(map.CellAt(max.x, y));
            }

            ICell centerCell = map.CellAt(center);
            borderCells.Remove(centerCell);

            return borderCells;
        }

        /// <summary>
        /// Get an IEnumerable of Cells in a line from the Origin Cell to the Destination Cell
        /// The resulting IEnumerable includes the Origin and Destination Cells
        /// Uses Bresenham's line algorithm to determine which Cells are in the closest approximation to a straight line between the two Cells
        /// </summary>
        /// <param name="origin">location of the Origin Cell at the start of the line</param>
        /// <param name="destination">location of the Destination Cell at the end of the line</param>
        /// <returns>IEnumerable of Cells in a line from the Origin Cell to the Destination Cell which includes the Origin and Destination Cells</returns>
        public static IEnumerable<ICell> CellsAlongLine(ICellContainer map, Vector2Int origin, Vector2Int destination)
        {
            Vector2Int maxPosition = new Vector2Int(map.Width - 1, map.Height - 1);
            origin.Clamp(Vector2Int.zero, maxPosition);
            destination.Clamp(Vector2Int.zero, maxPosition);

            Vector2Int d = new Vector2Int(
                Math.Abs(destination.x - origin.x),
                Math.Abs(destination.y - origin.y)
            );
            Vector2Int s = new Vector2Int(
                origin.x < destination.x ? 1 : -1,
                origin.y < destination.y ? 1 : -1
            );
            int err = d.x - d.y;

            while (true)
            {
                yield return map.CellAt(origin);
                if (origin == destination)
                    break;
                int e2 = 2 * err;
                if (e2 > -d.y)
                {
                    err = err - d.y;
                    origin.x += s.x;
                }
                if (e2 < d.x)
                {
                    err = err + d.x;
                    origin.y += s.y;
                }
            }
        }

        /// <summary>
        /// Get an IEnumerable of Cells in a square area around the center Cell up to the specified distance
        /// </summary>
        /// <param name="center">location of the center Cell</param>
        /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
        /// <returns>IEnumerable of Cells in a square area around the center Cell</returns>
        public static IEnumerable<ICell> CellsInSquare(ICellContainer map, Vector2Int center, int distance)
        {
            Vector2Int min = new Vector2Int(
                Math.Max(0, center.x - distance),
                Math.Max(0, center.y - distance)
            );
            Vector2Int max = new Vector2Int(
                Math.Min(map.Width - 1, center.x + distance),
                Math.Min(map.Height - 1, center.y + distance)
            );

            for (int x = min.x; x <= max.x; x++)
                for (int y = min.y; y <= max.y; y++)
                    yield return map.CellAt(x, y);
        }
    }
}