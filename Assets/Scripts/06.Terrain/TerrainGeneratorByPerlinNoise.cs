using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorByPerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 1.0f;
    public int octaves = 3;
    public float persistance = 0.5f;
    public float lacunarity = 2;

    private float xOrg = 0;
    private float yOrg = 0;

    public string seed;
    public bool useRandomSeed;
    public bool AutoUpdateMap;
    public bool useColorMap;
    public bool useGradientMap;

    [SerializeField]
    private PerlinNoise perlinNoise;
    [SerializeField]
    private GameObject cube;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (useRandomSeed) seed = Time.time.ToString();
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());
            xOrg = pseudoRandom.Next(0, 99999);
            yOrg = pseudoRandom.Next(0, 99999);
            GenerateMap();
        }
        if (AutoUpdateMap)
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        float[,] noiseMap = perlinNoise.GenerateMap(width, height, scale, octaves, persistance, lacunarity, xOrg, yOrg);
        CreateWorld(noiseMap);
    }

    public void CreateWorld(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Instantiate(cube, new Vector3(x, noiseMap[x, z], z), Quaternion.identity);
            }
        }
    }
}
