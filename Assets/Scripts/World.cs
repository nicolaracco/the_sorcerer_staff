using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sorcerer.Map;

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
    public PlayerEntity(Map map) : base(map, "@", map.playerStartPosition) { }
}

public class World
{
    public Map map { get; private set; }
    public Entity[] entities { get; private set; }
    public PlayerEntity playerEntity { get; private set; }

    public World(IMapGenerationOptions options)
    {
        map = Map.Generate(options);
        entities = new Entity[2];
        entities[0] = playerEntity = new PlayerEntity(map);
        entities[1] = new Entity(
            map, "@", new Vector2Int(map.width / 2 - 5, map.height / 2)
        );
    }
}