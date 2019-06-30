using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sorcerer.Map;

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
    public void OnMapGenerated(IMap map)
    {
        map.OnFovUpdate.AddListener(RefreshVisibleEntities);
        foreach (Entity entity in map.Entities)
            CreateEntity(entity);
        RefreshVisibleEntities(map);
    }

    private void RefreshVisibleEntities(IMap map)
    {
        foreach (EntityRenderer renderer in GetComponentsInChildren<EntityRenderer>(true))
            renderer.gameObject.SetActive(renderer.Entity.cell.isInFov);
    }

    /// <summary>
    /// Create a new renderer for the given entity
    /// </summary>
    /// <param name="entity">Entity for which the renderer will be created</param>
    private void CreateEntity(Entity entity)
    {
        GameObject instance = Instantiate(entityPrefab, Vector3.zero, Quaternion.identity);
        if (entity is PlayerEntity)
            instance.AddComponent<PlayerActor>();
        instance.transform.parent = transform;
        EntityRenderer renderer = instance.GetComponent<EntityRenderer>();
        renderer.Init(entity);
    }
}
