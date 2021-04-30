using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGeneratorByCellularAutomata : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [SerializeField]
    private string seed;
    [SerializeField]
    private bool useRandomSeed;

    [SerializeField]
    [Range(0, 100)]
    private int randomFillPercent;

    private int[,] map;
    private const int ROAD = 0;
    private const int WALL = 1;

    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tile tile;
    [SerializeField]
    private Color[] colors;

    private void Awake()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
            SmoothMap();
    }

    private void RandomFillMap()
    {
        if (useRandomSeed) seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = WALL;
                else map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD;
                OnDrawTile(x, y);
                SetTileColor(x, y);
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4) map[x, y] = WALL;
                else if (neighbourWallTiles < 4) map[x, y] = ROAD;
                SetTileColor(x, y);
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += map[neighbourX, neighbourY];
                }
                else wallCount++;
            }
        }
        return wallCount;
    }

    private void SetTileColor(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
        tilemap.SetTileFlags(pos, TileFlags.None);
        switch (map[x, y])
        {
            case ROAD: tilemap.SetColor(pos, colors[0]); break;
            case WALL: tilemap.SetColor(pos, colors[1]); break;
        }
    }

    private void OnDrawTile(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
        tilemap.SetTile(pos, tile);
    }
}
