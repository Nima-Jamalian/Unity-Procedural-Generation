using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProcGen02b : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;

    [Header("Background")]
    public Tile Tile;


    public bool[,] Road; // true if there is a road

    [Header("Roads")]
    public Tile[] RoadTiles;

    void Start()
    {
        FillBackground();

        Road = new bool[Size.x, Size.y];

        DrawHorizontalRoad();
        DrawHorizontalRoad();
        DrawHorizontalRoad();

        DrawVerticalRoad();
        DrawVerticalRoad();
        DrawVerticalRoad();

        DrawRoads();
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
            Road[x, y] = true;
            //Tilemap.SetTile(new Vector3Int(x, y, 0), RoadH);
    }
    // [2.2] Vertical
    // (if it overlaps a horizontal road, it makes a crossroad instead)
    void DrawVerticalRoad()
    {
        int x = Random.Range(0, Size.x);
        for (int y = 0; y < Size.y; y++)
            Road[x, y] = true;
    }

    void DrawRoads ()
    {
        /*
         *    N
         *   W+E
         *    S
         *    
         *  NESW
         *  1248
         */

        //for (int x = 0; x < Size.x; x++)
        //    for (int y = 0; y < Size.y; y++)
        for (int x = 1; x < Size.x-1; x++)
            for (int y = 1; y < Size.y-1; y++)
            {   
                // No road: nothing to do!
                if (!Road[x, y])
                        continue;

                int tileIndex = 0;

                if (Road[x    , y + 1]) tileIndex += 1;
                if (Road[x + 1, y    ]) tileIndex += 2;
                if (Road[x    , y - 1]) tileIndex += 4;
                if (Road[x - 1, y    ]) tileIndex += 8;

                Tilemap.SetTile(new Vector3Int(x, y, 0), RoadTiles[tileIndex]);
            }
    }
}
