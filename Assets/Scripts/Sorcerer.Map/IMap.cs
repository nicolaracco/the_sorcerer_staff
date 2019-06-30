using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sorcerer.Map
{
    [Serializable]
    public class MapEvent : UnityEvent<IMap> { }

    public interface IMap : ICellContainer
    {
        MapEvent OnFovUpdate { get; }

        Vector2Int PlayerStartPosition { get; set; }

        void ComputeFov(Vector2Int origin, int radius);
    }
}