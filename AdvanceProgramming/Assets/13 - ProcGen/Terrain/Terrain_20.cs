using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using AlanZucconi.ProcGen;

/* [TERRAIN 20]
 * Creates a random terrain using fBM noise
 * 
 * Creates water based on height.
 * 
 * Makes it into an island
 * 
 * Uses a gradient to give it colour based on height
 * 
 * Creates a river that moves down the landscape
 * 
 * Uses heightmap to create a Unity terrain.
 */
public class Terrain_20 : MonoBehaviour
{

    [Header("Texture")]
    public Renderer Renderer;
    public int Size;

    [Header("Perlin Noise")]
    public float OffsetX;
    public float OffsetY;
    [Min(0)]
    public float Scale;

    [Header("fBM Noise")]
    [Min(0)]
    public int Octaves = 8;
    [Range(0f, 1f)]
    public float Gain = 0.5f;

    [Space]
    [Range(0f,10f)]
    public float Power;

    [Header("Maps")]
    private float[,] HeightMap; // [x,y] = height
    private bool[,] WaterMap;   // [x,y] = true if it is water

    [Header("Water")]
    // 0: d=0       (center)     0
    // 1: d=size/2  (edge)       1
    public AnimationCurve IslandCurve;

    [Range(0f, 1f)]
    public float WaterThreshold;
    public Color WaterColor;

    [Header("Colour")]
    public Gradient HeightGradient;

    [Header("River")]
    [Min(0)]
    public int RiverCount = 10;
    [Min(0)]
    public int RiverLength;

    [Header("Unity Terrain")]
    public Terrain Terrain;

    // Start is called before the first frame update
    [Button(Editor=true)]
    void Start()
    {
        // [1] Random noise
        HeightMap = FBMNoise.Generate(Size, Size, OffsetX, OffsetY, Scale, Octaves, 0.5f);

        // [2] Power pass
        PowerPass();

        // [3] Makes it into an island
        IslandPass();        

        // [4] Water pass
        WaterMap = new bool[Size, Size];
        WaterPass();

        // [5] Creates a river
        for (int i = 0; i < RiverCount; i++)
        {
            Vector2Int riverStart = new Vector2Int(
                Random.Range((int)(Size * 0.1f), (int)(Size * 0.8f)),
                Random.Range((int)(Size * 0.1f), (int)(Size * 0.8f))
                );
            RiverPass(riverStart.x, riverStart.y);
        }

        // [6] Creates the texture
        TexturePass();

        // [7] Creates the terrain
        TerrainPass();
    }

    private void PowerPass()
    {
        int w = HeightMap.GetLength(0);
        int h = HeightMap.GetLength(1);

        // Normalises by the max value,
        // so that the new min is now 0
        // and the new max is now 1
        float min = HeightMap.Cast<float>().Min();
        float max = HeightMap.Cast<float>().Max();

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                // height:     [min, max]
                // new height: [0,   1]
                float height = (HeightMap[x, y] - min) / (max - min);
                HeightMap[x, y] = Mathf.Pow(height, Power);
            }
    }

    // The furthers you are from the center of the island,
    // the lower it gets.
    private void IslandPass()
    {
        int w = HeightMap.GetLength(0);
        int h = HeightMap.GetLength(1);

        Vector2 center = new Vector2(w / 2f, h / 2f);

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                Vector2 position = new Vector2(x, y);
                float distance = Vector2.Distance(center, position);

                HeightMap[x, y] *= IslandCurve.Evaluate(distance / (Size/2f));
            }
    }

    

    // Initialises the water map based on the height map
    private void WaterPass ()
    {
        int w = HeightMap.GetLength(0);
        int h = HeightMap.GetLength(1);

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                WaterMap[x, y] = HeightMap[x,y] <= WaterThreshold;
    }

    // Creates a river
    private void RiverPass(int x, int y)
    {
        // The directions in which the river can flow
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, +1),
            new Vector2Int(0, -1),
            new Vector2Int(+1, 0),
            new Vector2Int(-1, 0),

            new Vector2Int(+1, +1),
            new Vector2Int(+1, -1),
            new Vector2Int(-1, +1),
            new Vector2Int(-1, -1)
        };

        Vector2Int position = new Vector2Int(x, y);
        for (int i = 0; i < RiverLength; i ++)
        {
            WaterMap[position.x, position.y] = true;

            // Follows the down gradient
            // = finds the nearby cell with the lowest height, and goes there
            var dd = directions.Where
                (d =>
                {
                    Vector2Int target = position + d;
                    return !WaterMap[target.x, target.y];
                }
                );
            // No directions left?
            if (dd.Count() == 0)
                break;

            Vector2Int direction = dd
                // Selects only the directions which do not lead to water
                .MinBy(d =>
                    {
                        Vector2Int target = position + d;
                        return HeightMap[target.x, target.y];
                    });
            position += direction;

            // Stops if we touched the ocean
            if (HeightMap[position.x, position.y] <= WaterThreshold)
                break;
        }

    }

    private void TexturePass ()
    {
        int w = HeightMap.GetLength(0);
        int h = HeightMap.GetLength(1);

        Texture2D texture = new Texture2D(Size, Size);
        texture.filterMode = FilterMode.Bilinear;

        Color[] pixels = new Color[w*h];
        int i = 0;

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                Color c;

                if (WaterMap[x, y])
                    c = WaterColor;
                else
                {
                    float height = HeightMap[x, y];
                    c = HeightGradient.Evaluate(height);
                }

                pixels[i++] = c;
            }

        texture.SetPixels(pixels);
        texture.Apply();

        Renderer.material.mainTexture = texture;
    }

    private void TerrainPass ()
    {
        int w = HeightMap.GetLength(0);
        int h = HeightMap.GetLength(1);

        // HeightMap: [x,y]
        // unity terrain height: [y,x]
        float[,] heights = new float[h, w];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                heights[x, y] = HeightMap[y, x];
        
        
        Terrain.terrainData.SetHeights(0, 0, heights);
    }
}
