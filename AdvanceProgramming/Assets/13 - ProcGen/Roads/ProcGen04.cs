using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* [PROC GEN 04]
 * 
 * [1] Fills an entire section of the tilemap with a given tile.
 * [2] Creates a meandering river using randomwalk
 *      Uses a coroutine to create the river step by step
 */
public class ProcGen04 : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;

    [Header("Background")]
    public Tile Tile;

    [Header("Water")]
    public Tile Water; // Water tile
    public Vector2Int RiverStart;
    [Range(8,512*10)]
    public int RiverLength = 256;
    public Vector3Int[] Directions; // Directions the river can move to
    [Space]
    [Range(0f,1f)]
    public float Delay; // in seconds
    void Start()
    {
        FillBackground();

        StartCoroutine(CreateRiver());
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

    // [2] River with random walk
    IEnumerator CreateRiver()
    {
        Vector3Int position = (Vector3Int) RiverStart;
        for (int i = 0; i < RiverLength; i ++)
        {
            Tilemap.SetTile(position, Water);

            // Moves to the next position
            position += Directions[Random.Range(0, Directions.Length)];

            // Waits in between steps
            yield return new WaitForSeconds(Delay);
        }        
    }
}
