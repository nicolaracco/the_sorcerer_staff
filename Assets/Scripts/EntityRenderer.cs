using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sorcerer.Map;

/// <summary>
/// Generic entity renderer
/// It handles the entity update
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class EntityRenderer : MonoBehaviour
{
    [Serializable]
    public struct FaceSprite
    {
        public char symbol;
        public Sprite sprite;
    }

    public List<FaceSprite> availableFaces;

    [SerializeField]
    private Entity entity;

    public Entity Entity { get { return entity; } }

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
        FaceSprite faceSprite = availableFaces.FirstOrDefault(f => f.symbol == entity.symbol);
        if (faceSprite.sprite != null) // leave the default sprite if no face sprite is found
            spriteRenderer.sprite = faceSprite.sprite;
        spriteRenderer.color = entity.color;
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