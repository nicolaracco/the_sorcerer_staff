using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sorcerer.Map;

/// <summary>
/// The FloorTilemapRenderer renders the ground layer of the map
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class FloorTilemapRenderer : MonoBehaviour
{
    public Tile tile;

    public Color baseColor = new Color(50, 50, 150);
    public Color sightBlockedColor = new Color(0, 0, 100);
    public Color fovColor = new Color(0, 0, 0);

    private Tilemap tilemap;

    /// <summary>
    /// Triggered when the world has been generated
    /// It iterates over the world populating the tilemap
    /// </summary>
    /// <param name="world"></param>
    public void OnMapGenerated(IMap map)
    {
        map.OnFovUpdate.AddListener(Redraw);
        Redraw(map);
    }

    private void Redraw(IMap map)
    {
        for (int x = 0; x < map.Width; x++)
            for (int y = 0; y < map.Height; y++)
            {
                Cell cell = map.CellAt(x, y);
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, tile);
                tilemap.SetTileFlags(pos, TileFlags.None);
                if (cell.isInFov)
                    if (cell.isSightBlocked)
                        tilemap.SetColor(pos, sightBlockedColor);
                    else
                        tilemap.SetColor(pos, baseColor);
                else
                    tilemap.SetColor(pos, fovColor);
            }
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
}
