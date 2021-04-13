using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public float[,] GenerateMap(int width, int height, float scale, float octaves, float persistance, float lacunarity, float xOrg, float yOrg)
    {
        float[,] noiseMap = new float[width, height];
        scale = Mathf.Max(0.0001f, scale);
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = xOrg + x / scale * frequency;
                    float yCoord = yOrg + y / scale * frequency;
                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                noiseMap[x, y] = noiseHeight;
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0;y < height; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
