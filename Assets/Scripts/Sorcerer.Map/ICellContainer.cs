using UnityEngine;

namespace Sorcerer.Map
{
    public interface ICellContainer
    {
        int Width { get; }
        int Height { get; }

        Cell CellAt(Vector2Int position);
        Cell CellAt(int x, int y);
    }
}