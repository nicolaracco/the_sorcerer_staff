using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// The FloorTilemapRenderer renders the ground layer of the map
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class FloorTilemapRenderer : MonoBehaviour
{
    public Tile tile;

    public Color baseColor = new Color(50, 50, 150);
    public Color sightBlockedColor = new Color(0, 0, 100);

    private Tilemap tilemap;

    /// <summary>
    /// Triggered when the world has been generated
    /// It iterates over the world populating the tilemap
    /// </summary>
    /// <param name="world"></param>
    public void OnWorldGenerated(World world)
    {
        Map map = world.map;
        for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
            {
                Cell cell = map.cells[x, y];
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, tile);
                tilemap.SetTileFlags(pos, TileFlags.None);
                if (cell.isSightBlocked)
                    tilemap.SetColor(pos, sightBlockedColor);
                else
                    tilemap.SetColor(pos, baseColor);
            }
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
}
