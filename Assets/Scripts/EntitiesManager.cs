using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The EntitiesManager manages the rendering of entities in game
/// </summary>
public class EntitiesManager : MonoBehaviour
{
    public GameObject entityPrefab;

    /// <summary>
    /// Triggered when the world is generated
    /// It creates all the entities
    /// </summary>
    /// <param name="world"></param>
    public void OnWorldGenerated(World world)
    {
        foreach (Entity entity in world.entities)
            CreateEntity(entity);
    }

    /// <summary>
    /// Create a new renderer for the given entity
    /// </summary>
    /// <param name="entity">Entity for which the renderer will be created</param>
    private void CreateEntity(Entity entity)
    {
        GameObject instance = Instantiate(entityPrefab, Vector3.zero, Quaternion.identity);
        instance.transform.parent = transform;
        EntityRenderer renderer = instance.GetComponent<EntityRenderer>();
        renderer.Init(entity);
        if (entity is PlayerEntity)
            Camera.main.GetComponent<CameraController>().player = instance.transform;
    }
}
