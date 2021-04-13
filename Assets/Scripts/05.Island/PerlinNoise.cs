using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    
    public float xOrg;
    public float yOrg;
    
    public float scale = 1.0f;

    public string seed;
    public bool useRandomSeed;
    
    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    
    private void Awake()
    {
        noiseTex = new Texture2D(width, height);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend = GetComponent<Renderer>();
        rend.material.mainTexture = noiseTex;

    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) CalcNoise();
    }
    
    private void CalcNoise()
    {
        if (useRandomSeed) seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        xOrg = pseudoRandom.Next(0, 99999);
        yOrg = pseudoRandom.Next(0, 99999);

        for (float y = 0; y < noiseTex.height; y++)
        {
            for (float x = 0; x< noiseTex.width; x++)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)(y * noiseTex.width + x)] = new Color(sample, sample, sample);
            }
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
}
