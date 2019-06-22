using System;
using UnityEngine;

/// <summary>
/// Generic entity renderer
/// It handles the entity update
/// </summary>
public class EntityRenderer : MonoBehaviour
{
    public Entity entity { get; private set; }

    private bool needsUpdate = false;

    private Vector3 entityPositionToScreenPosition
    {
        get { return new Vector3(entity.position.x, entity.position.y, 0); }
    }

    /// <summary>
    /// Stores the entity reference, updates the transform position and register to entity events
    /// </summary>
    /// <param name="entity">The entity to be linked to the gameObject</param>
    public void Init(Entity entity)
    {
        this.entity = entity;
        this.entity.OnPositionChange.AddListener(OnEntityPositionChanged);
    }

    protected virtual void Start()
    {
        if (entity == null)
            throw new Exception("Missing entity in EntityController of " + gameObject.name);
        transform.position = entityPositionToScreenPosition;
        needsUpdate = false;
    }

    protected virtual void Update()
    {
        if (needsUpdate)
        {
            transform.position = entityPositionToScreenPosition;
            needsUpdate = false;
        }
    }

    /// <summary>
    /// Triggered when the entity position change. The next update after this call will update
    /// the entity
    /// </summary>
    private void OnEntityPositionChanged()
    {
        needsUpdate = true;
    }
}