using UnityEngine;
using Sorcerer.Map;

namespace Sorcerer
{
    /// <summary>
    /// The player entity
    /// </summary>
    public class PlayerEntity : Entity {
        private int fovRadius = 10;

        public PlayerEntity(IMap map) : base(map, "@", map.PlayerStartPosition) { }

        public override bool AttemptToMoveBy(Vector2Int delta)
        {
            if (base.AttemptToMoveBy(delta))
            {
                ComputeFov();
                return true;
            }
            return false;
        }

        public void ComputeFov()
        {
            map.ComputeFov(position, fovRadius);
        }
    }
}