using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sorcerer.Map
{
    [Serializable]
    public class MapEvent : UnityEvent<IMap> { }

    public interface IMap : ICellContainer, IEntitiesContainer
    {
        MapEvent OnFovUpdate { get; }

        void ComputeFov(Vector2Int origin, int radius);
    }
}