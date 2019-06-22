using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// World related event
/// </summary>
[Serializable]
public class WorldEvent : UnityEvent<World> {}

/// <summary>
/// The game manager handles the world generation and notifies for world related events
/// </summary>
public class GameManager : MonoBehaviour
{
    public WorldEvent OnWorldGenerate;

    public World world { get; private set; }
    public Entity playerEntity { get { return world.playerEntity; } }

    private void Awake()
    {
        world = new World();
    }

    private void Start()
    {
        OnWorldGenerate.Invoke(world);
    }
}