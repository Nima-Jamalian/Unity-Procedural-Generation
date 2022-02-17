using UnityEngine;
using System.Linq;

namespace AlanZucconi.ProcGen
{
    public static class WhiteNoise
    {
        // Range: [0, 1]
        public static float Sample ()
        {
            return Random.Range(0f, 1f);
        }

        #region Matrix
        public static void Generate(float[,] map)
        {
            int w = map.GetLength(0);
            int h = map.GetLength(1);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map[x, y] = Sample();
        }

        // Instantiates the matrix
        public static float[,] Generate (int w, int h)
        {
            float[,] map = new float[w, h];

            Generate(map);

            return map;
        }
        #endregion


        #region Texture
        public static void Generate (Texture2D texture)
        {
            int w = texture.width;
            int h = texture.height;

            Color[] pixels = Generate(w, h)
                .Cast<float>()
                .Select(s => new Color(s,s,s,1f))
                .ToArray();

            texture.SetPixels(pixels);
            texture.Apply();
        }

        public static Texture2D Generate (int w, int h, TextureFormat format = TextureFormat.ARGB32)
        {
            Texture2D texture = new Texture2D(w, h, format, false);
            texture.filterMode = FilterMode.Point;

            Generate(texture);

            return texture;
        }
        #endregion


        /*
        public static void Generate (Texture2D texture)
        {
            int w = texture.width;
            int h = texture.height;

            Color[] pixels = new Color[w * h];
            int i = 0;

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    float r = Sample();

                    Color c = new Color(r, r, r, 1f);
                    pixels[i++] = c;
                    //texture.SetPixel(x, y, c);
                }

            texture.SetPixels(pixels);
            texture.Apply();
        }
        */
    }
}
 