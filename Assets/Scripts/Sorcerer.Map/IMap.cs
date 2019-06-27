using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sorcerer.Map
{
    [Serializable]
    public class MapEvent : UnityEvent<IMap> { }

    public interface IMap
    {
        MapEvent OnFovUpdate { get; }

        int Width { get; }
        int Height { get; }

        Vector2Int PlayerStartPosition { get; set; }

        Cell CellAt(Vector2Int position);
        Cell CellAt(int x, int y);

        IEnumerable<ICell> BorderCellsInSquare(Vector2Int center, int distance);
        IEnumerable<ICell> CellsAlongLine(Vector2Int origin, Vector2Int destination);
        IEnumerable<ICell> CellsInSquare(Vector2Int center, int distance);

        void ComputeFov(Vector2Int origin, int radius);
    }
}