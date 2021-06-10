using UnityEngine;

public class IslandGeneratorByPerlinNoise : MonoBehaviour
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

    public bool useColorMap;
    public bool useGradientMap;

    [SerializeField] private PerlinNoise perlinNoise;
    [SerializeField] private Gradient gradient;
    [SerializeField] private MapDisplay mapDisplay;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (useRandomSeed) seed = Time.time.ToString(); //시드
            System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //의사 난수
            xOrg = pseudoRandom.Next(0, 99999); //의사 난수로 부터 랜덤 값 추출
            yOrg = pseudoRandom.Next(0, 99999);
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        float[,] noiseMap = perlinNoise.GenerateMap(width, height, scale, octaves, persistance, lacunarity, xOrg, yOrg); //노이즈 맵 생성
        float[,] gradientMap = gradient.GenerateMap(width, height); //그라디언트 맵 생성
        if (useGradientMap) mapDisplay.DrawNoiseMap(noiseMap, gradientMap, useColorMap); //노이즈 맵과 그라디언트 맵 결합
        else mapDisplay.DrawNoiseMap(noiseMap, noiseMap, useColorMap);
    }
}
