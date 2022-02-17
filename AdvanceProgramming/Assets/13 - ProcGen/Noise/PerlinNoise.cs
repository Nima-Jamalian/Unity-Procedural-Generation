using UnityEngine;
using System.Linq;

namespace AlanZucconi.ProcGen
{
    public static class PerlinNoise
    {
        // Range: [0, 1]
        // https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html
        public static float Sample(float x, float y)
        {
            return Mathf.Clamp01(Mathf.PerlinNoise(x, y));
        }

        #region Matrix
        public static void Generate(float[,] map,
            float offsetX = 0f, float offsetY = 0f,
            float scale = 1f
            )
        {
            int w = map.GetLength(0);
            int h = map.GetLength(1);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    float xCoord = offsetX + x / (float)w * scale;
                    float yCoord = offsetY + y / (float)h * scale;
                    map[x, y] = Sample(xCoord, yCoord);
                }
        }

        // Instantiates the matrix
        public static float[,] Generate(int w, int h,
            float offsetX = 0f, float offsetY = 0f,
            float scale = 1f
            )
        {
            float[,] map = new float[w, h];

            Generate(map, offsetX, offsetY, scale);

            return map;
        }
        #endregion


        #region Texture
        public static void Generate(Texture2D texture,
            float offsetX = 0f, float offsetY = 0f,
            float scale = 1
            )
        {
            int w = texture.width;
            int h = texture.height;

            Color[] pixels = Generate(w, h, offsetX, offsetY, scale)
                .Cast<float>()
                .Select(s => new Color(s, s, s, 1f))
                .ToArray();

            texture.SetPixels(pixels);
            texture.Apply();
        }

        public static Texture2D Generate(int w, int h,
            float offsetX = 0f, float offsetY = 0f,
            float scale = 1f,
            TextureFormat format = TextureFormat.ARGB32
            )
        {
            Texture2D texture = new Texture2D(w, h, format, false);
            texture.filterMode = FilterMode.Bilinear;

            Generate(texture, offsetX, offsetY, scale);

            return texture;
        }
        #endregion
    }
}
