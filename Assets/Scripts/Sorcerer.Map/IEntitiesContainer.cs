using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Sorcerer.Map
{
    public interface IEntitiesContainer
    {
        ReadOnlyCollection<Entity> Entities { get; }
        Entity Player { get; set; }

        void AddEntity(Entity entity);
        Entity FirstEntityAt(Vector2Int position, Func<Entity, bool> filter = null);
    }
}