using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sorcerer.Map
{
    /// <summary>
    /// The map
    /// </summary>
    public class Map
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
        
        public Cell[,] cells { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }

        /// <summary>
        /// Player starting position in the map
        /// </summary>
        public Vector2Int playerStartPosition = Vector2Int.zero;

        private Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            initMap();
        }

        public Dictionary<Vector2Int, Cell> GetConnectionsFrom(Vector2Int position)
        {
            Dictionary<Vector2Int, Cell> directions = new Dictionary<Vector2Int, Cell>();
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
            return directions;
        }

        private void initMap()
        {
            cells = new Cell[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    cells[x, y] = new Cell(this, new Vector2Int(x, y), true, true);
        }
    }
}