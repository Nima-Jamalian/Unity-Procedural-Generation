using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using System.Linq;

public enum TileConstraint
{
    Ground,
    Water,
    Both
}

[System.Serializable]
public struct TileInfo
{
    public Tile Tile;
    public TileConstraint SW;
    public TileConstraint S;
    public TileConstraint SE;
    public TileConstraint E;
    public TileConstraint NE;
    public TileConstraint N;
    public TileConstraint NW;
    public TileConstraint W;

    public bool IsCompatible (int n)
    {
        // No tile assigned for this = empty
        if (Tile == null)
            return false;

        var water_sw = (n & (1 << 0)) != 0;
        var water_s  = (n & (1 << 1)) != 0;
        var water_se = (n & (1 << 2)) != 0;
        var water_e = (n & (1 << 3)) != 0;
        var water_ne = (n & (1 << 4)) != 0;
        var water_n = (n & (1 << 5)) != 0;
        var water_nw = (n & (1 << 6)) != 0;
        var water_w = (n & (1 << 7)) != 0;

        // Must be compatible with all
        return
            IsCompatible(SW, water_sw) &&
            IsCompatible(S, water_s) &&
            IsCompatible(SE, water_se) &&
            IsCompatible(E, water_e) &&
            IsCompatible(NE, water_ne) &&
            IsCompatible(N, water_n) &&
            IsCompatible(NW, water_nw) &&
            IsCompatible(W, water_w)
            ;
    }

    static bool IsCompatible(TileConstraint tile, bool water)
    {
        if (tile == TileConstraint.Both)
            return true;

        if (tile == TileConstraint.Water && water)
            return true;

        if (tile == TileConstraint.Ground && !water)
            return true;

        return false;
    }

}
public class MarchingSquares : MonoBehaviour
{
    public TileInfo[] Tiles;

    public MarchingTiles Marching;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < Tiles.Length; i ++)
        for (int i = 0; i < 256; i++)
        {
            int compatibleCount = Tiles
                .Where(tile => tile.IsCompatible(i))
                .Count();

            Debug.Log(i + "\t" + compatibleCount);
            if (compatibleCount != 1)
                for (int t = 0; t < Tiles.Length; t++)
                    if (Tiles[t].IsCompatible(i))
                        Debug.Log("\t" + t);

            if (compatibleCount != 0)
            {
                Tile choosenTile = Tiles
                    .Where(tile => tile.IsCompatible(i))
                    .First()
                    .Tile;
                Marching.Tiles[i] = choosenTile;
            }
            
        }
    }
}
