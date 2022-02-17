using UnityEngine;
using System.Linq;

namespace AlanZucconi.ProcGen
{
    public static class FBMNoise
    {
        // With gain=0.5, it ranges in [0, 0.7...]
        // https://www.iquilezles.org/www/articles/fbm/fbm.htm
        // https://thebookofshaders.com/13/
        public static float Sample(float x, float y,
            // fBM
            int octaves = 8,
            float gain = 0.5f // [0,1] Gain
            )
        {
            // Fast version
            float f = 1.0f; // Frequenty
            float a = 0.5f; // Amplitude
            float t = 0.0f;
            for (int i = 0; i < octaves; i++)
            {
                //t += a * Noise(f * offsetX, f * offsetY);
                t += a * PerlinNoise.Sample(f * x, f * y);
                f *= 2.0f; // lacunarity
                a *= gain;
            }

            return t;

            /*
            float t = 0;
            for (int i = 0; i < octaves; i++)
            {
                float f = Mathf.Pow(2.0f, i);
                float a = Mathf.Pow(f, -H);
                t += a * Noise(f * offsetX, f * offsetY);
            }
            */
        }

        #region Matrix
        public static void Generate(float[,] map,
            // Perlin
            float offsetX, float offsetY,
            float scale = 1,
            // fBM
            int octaves = 8,
            float gain = 0.5f // [0,1] Gain
            )
        {
            int w = map.GetLength(0);
            int h = map.GetLength(1);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    float xCoord = offsetX + x / (float)w * scale;
                    float yCoord = offsetY + y / (float)h * scale;
                    map[x, y] = Sample(xCoord, yCoord, octaves, gain);
                }
        }

        // Instantiates the matrix
        public static float[,] Generate(int w, int h,
            // Perlin
            float offsetX, float offsetY,
            float scale = 1,
            // fBM
            int octaves = 8,
            float gain = 0.5f // [0,1] Gain
            )
        {
            float[,] map = new float[w, h];

            Generate(map, offsetX, offsetY, scale, octaves, gain);

            return map;
        }
        #endregion


        #region Texture
        public static void Generate(Texture2D texture,
            // Perlin
            float offsetX, float offsetY,
            float scale = 1,
            // fBM
            int octaves = 8,
            float gain = 0.5f // [0,1] Gain
            )
        {
            int w = texture.width;
            int h = texture.height;

            Color[] pixels = Generate(w, h, offsetX, offsetY, scale, octaves, gain)
                .Cast<float>()
                .Select(s => new Color(s, s, s, 1f))
                .ToArray();

            texture.SetPixels(pixels);
            texture.Apply();
        }

        public static Texture2D Generate(int w, int h,
            // Perlin
            float offsetX, float offsetY,
            float scale = 1,
            // fBM
            int octaves = 8,
            float gain = 0.5f, // [0,1] Gain

            TextureFormat format = TextureFormat.ARGB32
            )
        {
            Texture2D texture = new Texture2D(w, h, format, false);
            texture.filterMode = FilterMode.Bilinear;

            Generate(texture, offsetX, offsetY, scale, octaves, gain);

            return texture;
        }
        #endregion
    }
}
