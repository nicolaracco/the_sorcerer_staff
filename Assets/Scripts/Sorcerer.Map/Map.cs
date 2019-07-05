using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Sorcerer.Map.Generators;

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
            if (map.Player == null)
                throw new Exception("Player entity has not been registered during map generation");
            return map;
        }

        public MapEvent OnFovUpdate { get; private set; } = new MapEvent();

        public int Width { get; private set; }
        public int Height { get; private set; }

        private Cell[,] cells;
        private List<Entity> entities;
        
        private Entity player;
        public Entity Player { 
            get { return player; } 
            set {
                if (player != null)
                    throw new Exception("Player entity has already been registered in map");
                player = value;
                AddEntity(value);
            }
        }

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
            entities = new List<Entity>();
        }

        public Cell CellAt(Vector2Int position)
        {
            return cells[position.x, position.y];
        }
        public Cell CellAt(int x, int y)
        {
            return cells[x, y];
        }

        public ReadOnlyCollection<Entity> Entities
        {
            get
            {
                return new ReadOnlyCollection<Entity>(entities);
            }
        }

        public void AddEntity(Entity entity)
        {
            if (!entities.Contains(entity))
                entities.Add(entity);
        }

        public Entity FirstEntityAt(Vector2Int position, Func<Entity, bool> filter = null) {
            foreach (Entity e in entities)
                if (e.position == position && (filter == null || filter(e)))
                    return e;
            return null;
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
                cell.SetConnection(Direction.S, cells[position.x, position.y - 1]);
                if (position.x < cells.GetLength(0) - 1)
                    cell.SetConnection(Direction.SE, cells[position.x + 1, position.y - 1]);
                if (position.x > 0)
                    cell.SetConnection(Direction.SW, cells[position.x - 1, position.y - 1]);
            }
            if (position.y < cells.GetLength(1) - 1)
            {
                cell.SetConnection(Direction.N, cells[position.x, position.y + 1]);
                if (position.x < cells.GetLength(0) - 1)
                    cell.SetConnection(Direction.NE, cells[position.x + 1, position.y + 1]);
                if (position.x > 0)
                    cell.SetConnection(Direction.NW, cells[position.x - 1, position.y + 1]);
            }
            if (position.x < cells.GetLength(0) - 1)
                cell.SetConnection(Direction.E, cells[position.x + 1, position.y]);
            if (position.x > 0)
                cell.SetConnection(Direction.W, cells[position.x - 1, position.y]);
        }
    }
}