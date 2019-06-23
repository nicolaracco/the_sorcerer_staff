using System.Collections.Generic;
using UnityEngine;

namespace Sorcerer.Map
{
    /// <summary>
    /// Map cell
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Cell position in the map
        /// </summary>
        public Vector2Int position;
        /// <summary>
        /// true if this cell cannot be walked (lava, water, etc.)
        /// </summary>
        public bool isMovementBlocked;
        /// <summary>
        /// true if sight is blocked over this cell (walls, mountains, etc.)
        /// </summary>
        public bool isSightBlocked;

        private Map map;
        private Dictionary<Vector2Int, Cell> connections;

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
        public Dictionary<Vector2Int, Cell> Connections
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