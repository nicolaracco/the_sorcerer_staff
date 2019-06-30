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
                    cells[x, y] = new Cell(new Vector2Int(x, y), true, true);
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    PopulateCellConnections(cells[x, y]);
        }

        public Cell CellAt(Vector2Int position)
        {
            return cells[position.x, position.y];
        }
        public Cell CellAt(int x, int y)
        {
            return cells[x, y];
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

        private void PopulateCellConnections(Cell cell)
        {
            Vector2Int position = cell.position;
            if (position.y > 0)
            {
                cell.SetConnection(Direction.N, cells[position.x, position.y - 1]);
                if (position.x < cells.GetLength(0) - 1)
                    cell.SetConnection(Direction.NE, cells[position.x + 1, position.y - 1]);
                if (position.x > 0)
                    cell.SetConnection(Direction.NW, cells[position.x - 1, position.y - 1]);
            }
            if (position.y < cells.GetLength(1) - 1)
            {
                cell.SetConnection(Direction.S, cells[position.x, position.y + 1]);
                if (position.x < cells.GetLength(0) - 1)
                    cell.SetConnection(Direction.SE, cells[position.x + 1, position.y + 1]);
                if (position.x > 0)
                    cell.SetConnection(Direction.SW, cells[position.x - 1, position.y + 1]);
            }
            if (position.x < cells.GetLength(0) - 1)
                cell.SetConnection(Direction.E, cells[position.x + 1, position.y]);
            if (position.x > 0)
                cell.SetConnection(Direction.W, cells[position.x - 1, position.y]);
        }
    }
}