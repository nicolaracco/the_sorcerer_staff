using UnityEngine;
using Sorcerer.Map;
using Sorcerer.Map.Generators;

/// <summary>
/// The game manager handles the world generation and notifies for world related events
/// </summary>
public class GameManager : MonoBehaviour
{
    public MapEvent OnMapGenerate;

    public IMap map { get; private set; }
    public Entity playerEntity { get { return map.Player; } }

    private void Awake()
    {
        map = Map.Generate(new TutorialMapGenerationOptions(80, 50, 30, 6, 10, 3));
    }

    private void Start()
    {
        OnMapGenerate.Invoke(map);
    }
}