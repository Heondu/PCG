using UnityEngine;

public class IslandGeneratorByPerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float xOrg;
    public float yOrg;

    public float scale = 1.0f;

    public string seed;
    public bool useRandomSeed;

    [Range(0f, 1f)]
    public float[] fillPercents;
    public Color[] fillColors;

    private Texture2D noiseTex;
    public Texture2D gradientTex;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        noiseTex = new Texture2D(width, height);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(noiseTex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) GenerateTexture();
    }

    private void Init()
    {
        if (useRandomSeed) seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        xOrg = pseudoRandom.Next(0, 99999);
        yOrg = pseudoRandom.Next(0, 99999);
    }

    private void GenerateTexture()
    {
        Init();

        for (int x = 0; x < noiseTex.width; x++)
        {
            for (int y = 0; y < noiseTex.height; y++)
            {
                Color color = CalcColor(x, y);
                noiseTex.SetPixel(x, y, color);
            }
        }
        noiseTex.Apply();

    }

    private Color CalcColor(int x, int y)
    {
        float xCoord = xOrg + (float)x / noiseTex.width * scale;
        float yCoord = yOrg + (float)y / noiseTex.height * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        Color color = new Color(sample, sample, sample);
        color = color + gradientTex.GetPixel(x, y) - Color.white;

        for (int i = 0; i < fillPercents.Length; i++)
        {
            if (color.grayscale < fillPercents[i])
            {
                color = fillColors[i];
                break;
            }
        }
        return color;
    }
}
