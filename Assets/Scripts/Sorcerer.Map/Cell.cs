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

        private Map map;
        private ReadOnlyDictionary<Vector2Int, ICell> connections;

        public Cell(Map map, Vector2Int position, bool isMovementBlocked, bool isSightBlocked)
        {
            this.map = map;
            this.position = position;
            this.isMovementBlocked = isMovementBlocked;
            this.isSightBlocked = isSightBlocked;
        }

        /// <summary>
        /// Returns a memoized dictionary containing all the connections for this cell
        /// </summary>
        /// <value>The dictionary keys are vectors with delta values (eg. north is (0,-1))</value>
        public ReadOnlyDictionary<Vector2Int, ICell> Connections
        {
            get
            {
                if (connections != null)
                    return connections;
                connections = map.GetConnectionsFrom(position);
                return connections;
            }
        }
    }
}