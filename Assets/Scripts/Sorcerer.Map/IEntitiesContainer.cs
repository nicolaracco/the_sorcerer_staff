using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sorcerer.Map
{
    public interface IEntitiesContainer
    {
        ReadOnlyCollection<Entity> Entities { get; }
        PlayerEntity Player { get; }

        void AddEntity(Entity entity);
    }
}