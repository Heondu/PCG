using UnityEngine;

public class Gradient : MonoBehaviour
{
    [SerializeField] private Texture2D gradientTex;

    public float[,] GenerateMap(int width, int height)
    {
        float[,] gradientMap = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xCoord = Mathf.RoundToInt(x * (float)gradientTex.width / width); //텍스처 값과 크기 값에 맞춰 좌표 저장
                int yCoord = Mathf.RoundToInt(y * (float)gradientTex.height / height);
                gradientMap[x, y] = gradientTex.GetPixel(xCoord, yCoord).grayscale; //텍스처에서 색상을 가져와 그레이 스케일로 배열에 저장
            }
        }
        return gradientMap;
    }
}
