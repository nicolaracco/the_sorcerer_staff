using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Sorcerer.Map
{
    /// <summary>
    /// Map cell
    /// </summary>
    public class Cell : ICell
    {
        /// <summary>
        /// Cell position in the map
        /// </summary>
        public Vector2Int position { get; private set; }
        /// <summary>
        /// true if this cell cannot be walked (lava, water, etc.)
        /// </summary>
        public bool isMovementBlocked { get; set; }
        /// <summary>
        /// true if sight is blocked over this cell (walls, mountains, etc.)
        /// </summary>
        public bool isSightBlocked { get; set; }
        /// <summary>
        /// true if cell is in field of view
        /// </summary>
        public bool isInFov { get; set; } = false;

        private Dictionary<Direction, ICell> connections;

        public Cell(Vector2Int position, bool isMovementBlocked, bool isSightBlocked)
        {
            this.position = position;
            this.isMovementBlocked = isMovementBlocked;
            this.isSightBlocked = isSightBlocked;
            connections = new Dictionary<Direction, ICell>();
        }

        public void SetConnection(Direction direction, ICell destination)
        {
            connections.Add(direction, destination);
        }

        public ReadOnlyDictionary<Direction, ICell> Connections 
        { 
            get 
            { 
                return new ReadOnlyDictionary<Direction, ICell>(connections); 
            } 
        }
    }
}