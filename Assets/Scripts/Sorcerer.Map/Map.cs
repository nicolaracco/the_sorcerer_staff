using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Sorcerer.Map
{
    /// <summary>
    /// The map
    /// </summary>
    public class Map : IMap
    {
        public static Map Generate(IMapGenerationOptions options)
        {
            Map map = new Map(options.width, options.height);
            switch (options)
            {
                case TutorialMapGenerationOptions t:
                    new TutorialMapGenerator(map, t).Populate();
                    break;
                case MapGenerationOptions t:
                    new BaseMapGenerator(map, t).Populate();
                    break;
                default:
                    throw new Exception("Unknown map generation options received " + options.GetType().Name);
            }
            return map;
        }

        public MapEvent OnFovUpdate { get; private set; } = new MapEvent();

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Player starting position in the map
        /// </summary>
        public Vector2Int PlayerStartPosition { get; set; } = Vector2Int.zero;

        private Cell[,] cells;

        private Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            cells = new Cell[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    cells[x, y] = new Cell(this, new Vector2Int(x, y), true, true);
        }

        public Cell CellAt(Vector2Int position)
        {
            return cells[position.x, position.y];
        }
        public Cell CellAt(int x, int y)
        {
            return cells[x, y];
        }

        public ReadOnlyDictionary<Vector2Int, ICell> GetConnectionsFrom(Vector2Int position)
        {
            Dictionary<Vector2Int, ICell> directions = new Dictionary<Vector2Int, ICell>();
            if (position.y > 0)
            {
                directions.Add(new Vector2Int(0, -1), cells[position.x, position.y - 1]);
                if (position.x < cells.Length - 1)
                    directions.Add(new Vector2Int(1, -1), cells[position.x + 1, position.y - 1]);
                if (position.x > 0)
                    directions.Add(new Vector2Int(-1, -1), cells[position.x - 1, position.y - 1]);
            }
            if (position.y < cells.Length - 1)
            {
                directions.Add(new Vector2Int(0, 1), cells[position.x, position.y + 1]);
                if (position.x < cells.Length - 1)
                    directions.Add(new Vector2Int(1, 1), cells[position.x + 1, position.y + 1]);
                if (position.x > 0)
                    directions.Add(new Vector2Int(-1, 1), cells[position.x - 1, position.y + 1]);
            }
            if (position.x < cells.Length - 1)
                directions.Add(new Vector2Int(1, 0), cells[position.x + 1, position.y]);
            if (position.x > 0)
                directions.Add(new Vector2Int(-1, 0), cells[position.x - 1, position.y]);
            return new ReadOnlyDictionary<Vector2Int, ICell>(directions);
        }

        /// <summary>
        /// Get an IEnumerable of outermost border Cells in a square area around the center Cell up to the specified distance
        /// Taken from: https://github.com/FaronBracy/RogueSharp/blob/master/RogueSharp/Map.cs
        /// </summary>
        /// <param name="center">center Cell</param>
        /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
        /// <returns>IEnumerable of outermost border Cells in a square area around the center Cell</returns>
        public IEnumerable<ICell> BorderCellsInSquare(Vector2Int center, int distance)
        {
            Vector2Int min = new Vector2Int(
                Math.Max(0, center.x - distance),
                Math.Max(0, center.y - distance)
            );
            Vector2Int max = new Vector2Int(
                Math.Min(Width - 1, center.x + distance),
                Math.Min(Height - 1, center.y + distance)
            );
            List<ICell> borderCells = new List<ICell>();

            for (int x = min.x; x <= max.x; x++)
            {
                borderCells.Add(CellAt(x, min.y));
                borderCells.Add(CellAt(x, max.y));
            }
            for (int y = min.y + 1; y <= max.y - 1; y++)
            {
                borderCells.Add(CellAt(min.x, y));
                borderCells.Add(CellAt(max.x, y));
            }

            ICell centerCell = CellAt(center);
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
        public IEnumerable<ICell> CellsAlongLine(Vector2Int origin, Vector2Int destination)
        {
            Vector2Int maxPosition = new Vector2Int(Width - 1, Height - 1);
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
                yield return CellAt(origin);
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
        public IEnumerable<ICell> CellsInSquare(Vector2Int center, int distance)
        {
            Vector2Int min = new Vector2Int(
                Math.Max(0, center.x - distance),
                Math.Max(0, center.y - distance)
            );
            Vector2Int max = new Vector2Int(
                Math.Min(Width - 1, center.x + distance),
                Math.Min(Height - 1, center.y + distance)
            );

            for (int x = min.x; x <= max.x; x++)
                for (int y = min.y; y <= max.y; y++)
                    yield return CellAt(x, y);
        }

        /// <summary>
        /// Compute the field of view
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        public void ComputeFov(Vector2Int origin, int radius)
        {
            FieldOfView.Compute(this, origin, radius);
            OnFovUpdate.Invoke(this);
        }
    }
}