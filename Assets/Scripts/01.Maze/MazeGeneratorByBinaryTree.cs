using UnityEngine;

public class MazeGeneratorByBinaryTree : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private int[,] map;
    private const int ROAD = 0;
    private const int WALL = 1;

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
                if (x == 1 && y == 0) map[x, y] = ROAD;
                else if (x == width - 2 && y == height - 1) map[x, y] = ROAD;
                else if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = WALL;
                else if (x % 2 == 0 || y % 2 == 0) map[x, y] = WALL;
                else map[x, y] = ROAD;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % 2 == 0 || y % 2 == 0) continue;
                if (x == width - 2 && y == height - 2) continue;
                if (x == width - 2) map[x, y + 1] = ROAD;
                else if (y == height - 2) map[x + 1, y] = ROAD;
                else if (Random.Range(0, 2) == 0) map[x + 1, y] = ROAD;
                else map[x, y + 1] = ROAD;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == WALL) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
