using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* [PROC GEN 07]
 * 
 * [1] Fills an entire section of the tilemap with a given tile.
 * [2] Creates a meandering river using randomwalk
 *     - First places the water in a 2D matrix for easy access
 *     - Then loops through it to see get the right border tiles
 *     
 *     - Uses Marching Squares with "the blob"
 *     https://www.boristhebrave.com/2013/07/14/tileset-roundup/
 *     
 *     - Expands the river to make it "chunkier"
 *       It does it with an extra loop
 *     
 */
public class ProcGen07 : MonoBehaviour
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

    [Range(0,5)]
    public int Expand = 1; // How many times to expand

    public MarchingTiles WaterTiles;


    void Start()
    {
        FillBackground();

        // Create the 2D water matrix
        // to store where the water is
        // (it is easire to access than a tilemap!)
        Water = new bool[Size.x, Size.y]; // all false initially = no water

        CreateRiver();
        CreateRiver();
        CreateRiver();

        for (int i = 0; i < Expand; i++)
            ExpandRiver();

        RiverBorders();
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
        // [1] Creates the river in the water matrix
        Vector3Int position = (Vector3Int)RiverStart;
        position.y = Random.Range(0, Size.y);

        for (int i = 0; i < RiverLength; i++)
        {
            // Must not go outside the boundaries
            if (position.x >= 0 && position.x < Water.GetLength(0) &&
                position.y >= 0 && position.y < Water.GetLength(1))
                Water[position.x, position.y] = true;

            // Moves to the next position
            position += Directions[Random.Range(0, Directions.Length)];
        }
    }

    void ExpandRiver ()
    {
        // [2] Expands the river
        // Every cell next to water becomes water
        // Needs to do this in a new matrix
        bool[,] water = new bool[Size.x, Size.y];
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                water[x, y] = IsNearbyWater(x, y);
        Water = water;

    }
    private bool IsWater(int x, int y)
    {
        // Out of bounds? It is grass!
        if (x < 0 || x >= Water.GetLength(0) ||
            y < 0 || y >= Water.GetLength(1))
            return false;

        return Water[x, y];
    }

    private bool IsNearbyWater(int x, int y)
    {
        return
            IsWater(x, y) ||

            IsWater(x - 1, y - 1) ||
            IsWater(x - 1, y + 0) ||
            IsWater(x - 1, y + 1) ||

            IsWater(x + 0, y - 1) ||
            IsWater(x + 0, y + 0) ||
            IsWater(x + 0, y + 1) ||

            IsWater(x + 1, y - 1) ||
            IsWater(x + 1, y + 0) ||
            IsWater(x + 1, y + 1)
            ;
    }








    void RiverBorders ()
    { 
        // [3] Loops through all cells in the grid
        // Get the right tile based on which neighbour tiles have water
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
            {
                // Does nothing if there is no water in this cell
                if (!Water[x,y])
                    continue;

                // Finds the right tile to use
                // based on which neighbours have water
                Tilemap.SetTile
                (
                    new Vector3Int(x, y, 0),
                    WaterTiles.GetTile(Water, x, y)
                );
            }
    }
}