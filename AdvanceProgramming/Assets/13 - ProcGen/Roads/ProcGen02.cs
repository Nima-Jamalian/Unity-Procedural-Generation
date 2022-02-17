using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* [PROC GEN 02]
 * 
 * [1] Fills an entire section of the tilemap with a given tile.
 * [2] Draw a crossroad
 */
public class ProcGen02 : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;

    [Header("Background")]
    public Tile Tile;

    [Header("Roads")]
    public Tile RoadH; // Horizontal road tile
    public Tile RoadV; // Vertical road tile
    public Tile RoadX; // Cross road tile

    void Start()
    {
        FillBackground();

        DrawHorizontalRoad();
        DrawHorizontalRoad();
        DrawHorizontalRoad();

        DrawVerticalRoad();
        DrawVerticalRoad();
        DrawVerticalRoad();
    }

    // [1] Fills in the background
    void FillBackground()
    {
        // Loops through all the locations in the area of interest
        // and sets the tile
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                Tilemap.SetTile(new Vector3Int(x, y, 0), Tile);
    }

    // [2] Draws a crossroad
    // [2.1] Horizontal
    void DrawHorizontalRoad()
    {
        int y = Random.Range(0, Size.y);
        for (int x = 0; x < Size.x; x++)
            Tilemap.SetTile(new Vector3Int(x, y, 0), RoadH);
    }
    // [2.2] Vertical
    // (if it overlaps a horizontal road, it makes a crossroad instead)
    void DrawVerticalRoad()
    {
        int x = Random.Range(0, Size.x);
        for (int y = 0; y < Size.y; y++)
        {
            Vector3Int position = new Vector3Int(x, y, 0);
            if (Tilemap.GetTile(position) == RoadH)
                Tilemap.SetTile(position, RoadX);
            else
                Tilemap.SetTile(position, RoadV);
        }
    }
}
