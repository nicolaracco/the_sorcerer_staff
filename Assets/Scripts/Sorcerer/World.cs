using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sorcerer.Map;

namespace Sorcerer
{
    [Serializable]
    public class WorldEvent : UnityEvent<World> {}

    public class World
    {
        public WorldEvent OnFovUpdate = new WorldEvent();
        
        public IMap map { get; private set; }
        public Entity[] entities { get; private set; }
        public PlayerEntity playerEntity { get; private set; }

        public World(IMapGenerationOptions options)
        {
            map = Sorcerer.Map.Map.Generate(options);
            map.OnFovUpdate.AddListener(TriggerFovUpdate);
            entities = new Entity[2];
            entities[0] = playerEntity = new PlayerEntity(map);
            playerEntity.ComputeFov();
            entities[1] = new Entity(
                map, "@", new Vector2Int(map.Width / 2 - 5, map.Height / 2)
            );
        }

        private void TriggerFovUpdate(IMap map)
        {
            OnFovUpdate.Invoke(this);
        }
    }
}