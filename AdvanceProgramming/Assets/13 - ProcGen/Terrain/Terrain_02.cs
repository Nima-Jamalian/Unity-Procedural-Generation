using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using AlanZucconi.ProcGen;

/* [TERRAIN 02]
 * Creates a random terrain using fBM noise
 * 
 * Uses power function to make the terrain more extreme.
 */
public class Terrain_02 : MonoBehaviour
{

    [Header("Texture")]
    public Renderer Renderer;
    public int Size;

    [Header("Perlin Noise")]
    public float OffsetX;
    public float OffsetY;
    [Min(0)]
    public float Scale = 4f;

    [Header("fBM Noise")]
    [Min(0)]
    public int Octaves = 8;
    [Range(0f, 1f)]
    public float Gain = 0.5f;

    [Header("Terrain")]
    [Range(0f, 8f)]
    public float Power = 1f;

    

    private float[,] HeightMap; // [x,y] = height

    // Start is called before the first frame update
    [Button(Editor = true)]
    void Start()
    {
        // [1] Random noise
        HeightMap = FBMNoise.Generate(Size, Size, OffsetX, OffsetY, Scale, Octaves, Gain);

        // [2] Power pass
        PowerPass();

        // [3] Makes the heightmap into a texture
        TexturePass();
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

    private void TexturePass()
    {
        int w = HeightMap.GetLength(0);
        int h = HeightMap.GetLength(1);

        Texture2D texture = new Texture2D(Size, Size);
        texture.filterMode = FilterMode.Bilinear;

        Color[] pixels = new Color[w * h];
        int i = 0;

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float s = HeightMap[x, y];
                Color c = new Color(s, s, s, 1f);

                pixels[i++] = c;
            }

        texture.SetPixels(pixels);
        texture.Apply();

        Renderer.material.mainTexture = texture;
    }
}
