using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public float[,] GenerateMap(int width, int height, float scale, float octaves, float persistance, float lacunarity, float xOrg, float yOrg)
    {
        float[,] noiseMap = new float[width, height];
        scale = Mathf.Max(0.0001f, scale);
        float maxNoiseHeight = float.MinValue; //최대 값을 담기 위한 변수
        float minNoiseHeight = float.MaxValue; //최소 값을 담기 위한 변수
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1; //진폭. 노이즈의 폭과 관련된 값.
                float frequency = 1; //주파수. 노이즈의 간격과 관련된 값. 주파수가 커질수록 노이즈가 세밀해짐
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) //옥타브가 증가할수록 높은 주파수와 낮은 진폭의 노이즈가 중첩됨.
                {
                    float xCoord = xOrg + x / scale * frequency;
                    float yCoord = yOrg + y / scale * frequency;
                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1; //0~1 사이의 값을 반환하는 함수. 2를 곱하고 1을 빼서 -1~1 사이의 값으로 변환
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
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //lerp의 역함수로 최솟값과 최댓값의 사잇값을 3번째 인자로 넣으면 0~1사이의 값을 반환
            }
        }
        return noiseMap;
    }
}
