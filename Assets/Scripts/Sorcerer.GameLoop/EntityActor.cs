using System;
using Sorcerer.Map;

namespace Sorcerer.GameLoop
{
    public abstract class EntityActor : AGameLoopActor
    {
        public IMap map { get; private set; }
        public Entity entity { get; private set; }

        public void Init(IMap map, Entity entity)
        {
            if (this.map != null || this.entity != null)
                throw new Exception("Entity has already been assigned!");
            this.map = map;
            this.entity = entity;
        }
    }
}