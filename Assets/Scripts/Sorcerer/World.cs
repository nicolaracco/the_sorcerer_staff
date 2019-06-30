using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sorcerer.Map;
using Sorcerer.Map.Generators;

namespace Sorcerer
{
    [Serializable]
    public class WorldEvent : UnityEvent<World> {}

    public class World
    {
        public WorldEvent OnFovUpdate = new WorldEvent();
        
        public IMap map { get; private set; }

        public World(IMapGenerationOptions options)
        {
            map = Sorcerer.Map.Map.Generate(options);
            map.OnFovUpdate.AddListener(TriggerFovUpdate);
        }

        private void TriggerFovUpdate(IMap map)
        {
            OnFovUpdate.Invoke(this);
        }
    }
}