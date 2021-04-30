using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGeneratorByBinaryTree : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private int[,] map;
    private const int ROAD = 0;
    private const int WALL = 1;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tile tile;
    [SerializeField]
    private Color[] colors;

    private void Update()
    {
        Debug.Assert(!(width % 2 == 0 || height % 2 == 0), "홀수로 입력하십시오.");
        if (Input.GetMouseButtonDown(0)) Generate();
    }

    private void Generate()
    {
        map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                OnDrawTile(x, y);
                SetTileColor(x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 1 && y == 0) map[x, y] = ROAD;
                else if (x == width - 2 && y == height - 1) map[x, y] = ROAD;
                else if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = WALL;
                else if (x % 2 == 0 || y % 2 == 0) map[x, y] = WALL;
                else map[x, y] = ROAD;
                SetTileColor(x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos;
                if (x % 2 == 0 || y % 2 == 0) continue;
                if (x == width - 2 && y == height - 2) continue;
                if (x == width - 2) pos = new Vector2Int(x, y + 1);
                else if (y == height - 2) pos = new Vector2Int(x + 1, y);
                else if (Random.Range(0, 2) == 0) pos = new Vector2Int(x + 1, y);
                else pos = new Vector2Int(x, y + 1);
                map[pos.x, pos.y] = ROAD;
                SetTileColor(pos.x, pos.y);
            }
        }
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
