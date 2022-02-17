using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Marching Tiles", menuName = "ProcGen/Marching Tiles", order = 1)]
public class MarchingTiles : ScriptableObject
{
    // https://www.boristhebrave.com/2013/07/14/tileset-roundup/
    public Tile[] Tiles;

    public Tile GetTile (bool [,] matrix, int x, int y)
    {
        // 64--32--16
        // |       |
        // 128     8
        // |       |
        // 1---2---4
        //    
        int tileId = 0;
        if (Is(matrix, x - 1, y - 1)) tileId += 1;      // ⭩
        if (Is(matrix, x + 0, y - 1)) tileId += 2;      // ⭣
        if (Is(matrix, x + 1, y - 1)) tileId += 4;      // ⭨
        if (Is(matrix, x + 1, y + 0)) tileId += 8;      // ⭢
        if (Is(matrix, x + 1, y + 1)) tileId += 16;     // ⭧
        if (Is(matrix, x + 0, y + 1)) tileId += 32;     // ⭡
        if (Is(matrix, x - 1, y + 1)) tileId += 64;     // ⭦
        if (Is(matrix, x - 1, y + 0)) tileId += 128;    // ⭠

        return Tiles[tileId];
    }

    private bool Is (bool [,] matrix, int x, int y)
    {
        // Out of bounds? It is grass!
        if (x < 0 || x >= matrix.GetLength(0) ||
            y < 0 || y >= matrix.GetLength(1) )
            return false;

        return matrix[x, y];
    }
}
