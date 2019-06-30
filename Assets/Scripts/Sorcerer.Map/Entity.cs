using System;
using UnityEngine;
using UnityEngine.Events;
using Sorcerer.Map;

namespace Sorcerer.Map
{
    /// <summary>
    /// Map entity. It represents npcs, items, etc.
    /// </summary>
    [Serializable]
    public class Entity
    {
        private Vector2Int _position;
        protected readonly IMap map;

        /// <summary>
        /// Event triggered when the entity position changes
        /// </summary>
        public UnityEvent OnPositionChange = new UnityEvent();
        /// <summary>
        /// Character representing the entity
        /// </summary>
        public char symbol;
        /// <summary>
        /// Entity name
        /// </summary>
        public string name;
        /// <summary>
        /// Entity color
        /// </summary>
        public Color color;
        /// <summary>
        /// Entity position in the map
        /// </summary>
        /// <value></value>
        public Vector2Int position
        {
            get { return _position; }
            private set 
            {
                _position = value;
                OnPositionChange.Invoke();
            }
        }

        public Cell cell { get { return map.CellAt(position); } }

        public Entity(IMap map, char symbol, Color color, string name, Vector2Int position)
        {
            this.map = map;
            this.name = name;
            this.position = position;
            this.color = color;
            this.symbol = symbol;
        }

        /// <summary>
        /// Attempt to move the entity by a step
        /// </summary>
        /// <param name="delta">A vector representing the position delta (eg. (0,-1) for moving north)</param>
        /// <returns>true if the movement can be made</returns>
        public virtual bool AttemptToMoveBy(Vector2Int delta)
        {
            ICell connection;
            if (!cell.Connections.TryGetValue(delta.ToSorcererDirection(), out connection))
                return false;
            if (connection.isMovementBlocked)
                return false;
            position += delta;
            return true;
        }
    }
}