using System;
using UnityEngine;
using UnityEngine.Events;
using Sorcerer.Map;

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
        world = new World(new TutorialMapGenerationOptions(80, 50, 30, 6, 10));
    }

    private void Start()
    {
        OnWorldGenerate.Invoke(world);
    }
}