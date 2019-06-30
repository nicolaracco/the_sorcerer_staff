using System;
using UnityEngine;
using Sorcerer.Map;

/// <summary>
/// Generic entity renderer
/// It handles the entity update
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class EntityRenderer : MonoBehaviour
{
    public Color playerColor = Color.white;
    public Color npcColor = Color.yellow;

    public Entity entity { get; private set; }

    private SpriteRenderer spriteRenderer;
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
        spriteRenderer.color = entity is PlayerEntity ? playerColor : npcColor;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (entity == null)
            throw new Exception("Missing entity in EntityController of " + gameObject.name);
        transform.position = entityPositionToScreenPosition;
        needsUpdate = false;
    }

    private void Update()
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