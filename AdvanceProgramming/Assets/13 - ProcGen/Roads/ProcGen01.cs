using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* [PROC GEN 01]
 * 
 * Fills an entire section of the tilemap with a given tile.
 */
public class ProcGen01 : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;

    public Tile Tile;

    void Start()
    {
        // Loops through all the locations in the area of interest
        // and sets the tile
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                Tilemap.SetTile(new Vector3Int(x,y, 0), Tile);
    }
}
