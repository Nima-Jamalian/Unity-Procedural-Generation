using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.ProcGen;

/* [TERRAIN 01]
 * Creates a random terrain using fBM noise
 *
 */
public class Terrain_01 : MonoBehaviour
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

    

    private float[,] HeightMap; // [x,y] = height


    // Start is called before the first frame update
    [Button(Editor = true)]
    void Start()
    {
        // [1] Random noise
        HeightMap = FBMNoise.Generate(Size, Size, OffsetX, OffsetY, Scale, Octaves, Gain);

        // [2] Makes the heightmap into a texture
        TexturePass();
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
