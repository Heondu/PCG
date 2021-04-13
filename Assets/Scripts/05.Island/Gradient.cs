using UnityEngine;

public class Gradient : MonoBehaviour
{
    [SerializeField]
    private Texture2D gradientTex;

    public float[,] GenerateMap(int width, int height)
    {
        float[,] gradientMap = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xCoord = Mathf.RoundToInt(x * (float)gradientTex.width / width);
                int yCoord = Mathf.RoundToInt(y * (float)gradientTex.height / height);
                gradientMap[x, y] = gradientTex.GetPixel(xCoord, yCoord).grayscale;
            }
        }
        return gradientMap;
    }
}
