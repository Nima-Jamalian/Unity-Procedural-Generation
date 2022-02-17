using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AlanZucconi.ProcGen;
public class ProGenNoise : MonoBehaviour
{
    [SerializeField] float[,] heightMap;
    [SerializeField] Texture2D texture;
    [SerializeField] Gradient gradient;
    [SerializeField] Renderer renderer;
    [Header("Paramaters")]
    [Range(0, 10)]
    [SerializeField] float power = 2;
    [Range(0, 10)]
    [SerializeField] float xOffest = 2;
    [Range(0, 10)]
    [SerializeField] float yOffset = 2;
    // Start is called before the first frame update
    void Update()
    {
        texture =  PerlinNoise.Generate(128, 128,0,0,1,TextureFormat.ARGB32);
        heightMap = FBMNoise.Generate(128*2, 128*2, xOffest, yOffset, 10, 8, 0.5f);
        RemapPass();
        PowerPass();
        TexturePass();
    }

    void RemapPass()
    {
        int w = heightMap.GetLength(0);
        int h = heightMap.GetLength(1);
        //[min, max] --> [0, 1]
        float min = heightMap.Cast<float>().Min();
        float max = heightMap.Cast<float>().Max();
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                heightMap[x, y] = (heightMap[x, y] - min) / (max - min);
            }
        }
    }

    void PowerPass()
    {
        int w = heightMap.GetLength(0);
        int h = heightMap.GetLength(1);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                heightMap[x, y] = Mathf.Pow(heightMap[x, y], power);
            }
        }
    }

    void TexturePass()
    {
        int w = heightMap.GetLength(0);
        int h = heightMap.GetLength(1);
        texture = new Texture2D(w, h);
        texture.filterMode = FilterMode.Bilinear;

        for(int x=0; x<w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                //Color c = new Color(heightMap[x, y], heightMap[x, y], heightMap[x, y]);
                Color c = gradient.Evaluate(heightMap[x,y]);
                texture.SetPixel(y, x, c);
            }
        }
        texture.Apply();

        renderer.material.mainTexture = texture;
    }
}
