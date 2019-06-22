using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

/// <summary>
/// Map entity. It represents npcs, items, etc.
/// </summary>
public class Entity
{
    private Vector2Int _position;
    private Map map;

    /// <summary>
    /// Event triggered when the entity position changes
    /// </summary>
    public UnityEvent OnPositionChange = new UnityEvent();
    /// <summary>
    /// Entity name
    /// </summary>
    public string name;
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

    public Cell cell { get { return map.cells[position.x, position.y]; } }

    public Entity(Map map, string name, Vector2Int position)
    {
        this.map = map;
        this.name = name;
        this.position = position;
    }

    /// <summary>
    /// Attempt to move the entity by a step
    /// </summary>
    /// <param name="delta">A vector representing the position delta (eg. (0,-1) for moving north)</param>
    /// <returns>true if the movement can be made</returns>
    public bool AttemptToMoveBy(Vector2Int delta)
    {
        Cell connection;
        if (!cell.Connections.TryGetValue(delta, out connection))
            return false;
        if (connection.isMovementBlocked)
            return false;
        position += delta;
        return true;
    }
}

/// <summary>
/// The player entity
/// </summary>
public class PlayerEntity : Entity {
    public PlayerEntity(Map map, Vector2Int position) : base(map, "@", position) { }
}

/// <summary>
/// The map
/// </summary>
public class Map
{
    public Cell[,] cells { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }

    public Map(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                cells[x, y] = new Cell(this, new Vector2Int(x, y), false, false);

        for (int x = 30; x < 33; x++)
            cells[x, 22].isMovementBlocked = cells[x, 22].isSightBlocked = true;
    }

    public Dictionary<Vector2Int, Cell> GetConnectionsFrom(Vector2Int position)
    {
        Dictionary<Vector2Int, Cell> directions = new Dictionary<Vector2Int, Cell>();
        if (position.y > 0)
        {
            directions.Add(new Vector2Int(0, -1), cells[position.x, position.y - 1]);
            if (position.x < cells.Length - 1)
                directions.Add(new Vector2Int(1, -1), cells[position.x + 1, position.y - 1]);
            if (position.x > 0)
                directions.Add(new Vector2Int(-1, -1), cells[position.x - 1, position.y - 1]);
        }
        if (position.y < cells.Length - 1)
        {
            directions.Add(new Vector2Int(0, 1), cells[position.x, position.y + 1]);
            if (position.x < cells.Length - 1)
                directions.Add(new Vector2Int(1, 1), cells[position.x + 1, position.y + 1]);
            if (position.x > 0)
                directions.Add(new Vector2Int(-1, 1), cells[position.x - 1, position.y + 1]);
        }
        if (position.x < cells.Length - 1)
            directions.Add(new Vector2Int(1, 0), cells[position.x + 1, position.y]);
        if (position.x > 0)
            directions.Add(new Vector2Int(-1, 0), cells[position.x - 1, position.y]);
        return directions;
    }
}

public class World
{
    public Map map { get; private set; }
    public Entity[] entities { get; private set; }
    public PlayerEntity playerEntity { get; private set; }

    public World()
    {
        map = new Map(80, 50);
        entities = new Entity[1];
        entities[0] = playerEntity = new PlayerEntity(
            map, new Vector2Int(map.width / 2, map.height / 2)
        );
    }
}