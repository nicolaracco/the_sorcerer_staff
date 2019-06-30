using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Sorcerer.Map
{
    public interface ICell
    {
        Vector2Int position { get; }
        bool isSightBlocked { get; }
        bool isMovementBlocked { get; }
        bool isInFov { get; set; }

        ReadOnlyDictionary<Direction, ICell> Connections { get; }
    }
}