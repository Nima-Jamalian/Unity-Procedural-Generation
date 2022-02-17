using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* [PROC GEN 06]
 * 
 * [1] Fills an entire section of the tilemap with a given tile.
 * [2] Creates a meandering river using randomwalk
 *     - First places the water in a 2D matrix for easy access
 *     - Then loops through it to see get the right border tiles
 *     
 *     - Uses Marching Squares with "the blob"
 *     https://www.boristhebrave.com/2013/07/14/tileset-roundup/
 *     
 */
public class ProcGen06 : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;

    [Header("Background")]
    public Tile Tile;

    [Header("Water")]
    //public Tile Water; // Water tile
    public Vector2Int RiverStart;
    [Range(8,512*10)]
    public int RiverLength = 256;
    public Vector3Int[] Directions; // Directions the river can move to

    private bool[,] Water; // [x,y] true if there is water at [x,y]

    public MarchingTiles WaterTiles;

    void Start()
    {
        FillBackground();

        CreateRiver();
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
    void CreateRiver()
    {
        // Create the 2D water matrix
        // to store where the water is
        // (it is easire to access than a tilemap!)
        Water = new bool[Size.x, Size.y]; // all false initially = no water

        // [1] Creates the river in the water matrix
        Vector3Int position = (Vector3Int) RiverStart;
        for (int i = 0; i < RiverLength; i ++)
        {
            // Must not go outside the boundaries
            if (position.x >= 0 && position.x < Water.GetLength(0) &&
                position.y >= 0 && position.y < Water.GetLength(1) )
                Water[position.x, position.y] = true;

            // Moves to the next position
            position += Directions[Random.Range(0, Directions.Length)];
        }

        // [2] Loops through all cells in the grid
        // Get the right tile based on which neighbour tiles have water
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
            {
                // Does nothing if there is no water in this cell
                //if (!IsWater(x, y))
                if (!Water[x,y])
                    continue;

                // Finds the right tile to use
                // based on which neighbours have water
                Tilemap.SetTile(new Vector3Int(x, y, 0), WaterTiles.GetTile(Water, x, y));
            }
    }
    /*
    private bool IsWater (int x, int y)
    {
        // Out of bounds? It is grass!
        if (x < 0 || x >= Water.GetLength(0) ||
            y < 0 || y >= Water.GetLength(1) )
            return false;

        return Water[x, y];
    }
    */
    /*
    // Based on the nearby tiles, it finds the right corner tile
    // (assumes tile at [x,y] is water)
    private Tile GetWaterTile (int x, int y)
    {
        // 1---2---4
        // |       |
        // 8       16
        // |       |
        // 32--64--128
        //        
        int tileId = 0;
        if (IsWater(x - 1, y + 1)) tileId += 1;     // ⭦
        if (IsWater(x + 0, y + 1)) tileId += 2;     // ⭡
        if (IsWater(x + 1, y + 1)) tileId += 4;     // ⭧
        if (IsWater(x - 1, y + 0)) tileId += 8;     // ⭠
        if (IsWater(x + 1, y + 0)) tileId += 16;    // ⭢
        if (IsWater(x - 1, y - 1)) tileId += 32;    // ⭩
        if (IsWater(x + 0, y - 1)) tileId += 64;    // ⭣
        if (IsWater(x + 1, y - 1)) tileId += 128;   // ⭨

        return WaterTiles.Tiles[tileId];
    }
    */
}
