using UnityEngine;
using System.Linq;

namespace AlanZucconi.ProcGen
{
    public static class RGBNoise
    {
        // Range: [0, 1]
        public static Vector3 Sample()
        {
            return new Vector3
                (
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f)
                );
        }

        #region Matrix
        public static void Generate(Vector3[,] map)
        {
            int w = map.GetLength(0);
            int h = map.GetLength(1);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map[x, y] = Sample();
        }

        // Instantiates the matrix
        public static Vector3[,] Generate(int w, int h)
        {
            Vector3[,] map = new Vector3[w, h];

            Generate(map);

            return map;
        }
        #endregion


        #region Texture
        public static void Generate(Texture2D texture)
        {
            int w = texture.width;
            int h = texture.height;

            Color[] pixels = Generate(w, h)
                .Cast<Vector3>()
                .Select(v => new Color(v.x, v.y, v.z, 1f))
                .ToArray();

            texture.SetPixels(pixels);
            texture.Apply();
        }

        public static Texture2D Generate(int w, int h, TextureFormat format = TextureFormat.ARGB32)
        {
            Texture2D texture = new Texture2D(w, h, format, false);
            texture.filterMode = FilterMode.Point;

            Generate(texture);

            return texture;
        }
        #endregion
    }
}
