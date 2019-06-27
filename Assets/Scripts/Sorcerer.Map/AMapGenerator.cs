using UnityEngine;

namespace Sorcerer.Map
{
    public abstract class AMapGenerator<T> where T : IMapGenerationOptions
    {
        public Map map { get; private set; }
        public T options { get; private set; }

        public Cell CellAt(Vector2Int position) { return map.CellAt(position); }
        public Cell CellAt(int x, int y) { return map.CellAt(x, y); }

        public AMapGenerator(Map map, T options)
        {
            this.map = map;
            this.options = options;
        }

        public abstract void Populate();
    }
}