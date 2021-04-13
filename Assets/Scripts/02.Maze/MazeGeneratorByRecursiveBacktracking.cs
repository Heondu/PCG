using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TILE {ROAD = 0, WALL, CHECK, START};

public class MazeGeneratorByRecursiveBacktracking : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    private int[,] map;
    private Vector2Int[] direction = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private Vector2Int pos = Vector2Int.zero;
    private Stack<Vector2Int> stackDir = new Stack<Vector2Int>();

    private void Update()
    {
        Debug.Assert(!(width == 0 || height == 0), "크기를 입력하십시오.");
        if (Input.GetMouseButtonDown(0)) Generate();
    }

    public void Generate()
    {
        Init();
        RandPosSelect();
        RandDirection();
        StartCoroutine("Check");
    }

    private void Init()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (int)TILE.WALL;
            }
        }
    }

    private void RandPosSelect()
    {
        do
        {
            pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (pos.x % 2 == 0 || pos.y % 2 == 0);
    }

    private void RandDirection()
    {
        for (int i = 0; i < direction.Length; i++)
        {
            int randNum = Random.Range(0, direction.Length);
            Vector2Int temp = direction[randNum];
            direction[randNum] = direction[i];
            direction[i] = temp;
        }
    }

    private IEnumerator Check()
    {
        map[pos.x, pos.y] = (int)TILE.START;
        do
        {
            int index = -1;

            RandDirection();

            for (int i = 0; i < direction.Length; i++)
            {
                if (CheckCondition(i)) index = i;
            }

            if (index != -1)
            {
                for (int i = 0; i < 2; i++)
                {
                    stackDir.Push(direction[index]);
                    pos += direction[index];
                    map[pos.x, pos.y] = (int)TILE.CHECK;
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    map[pos.x, pos.y] = (int)TILE.ROAD;
                    pos += stackDir.Pop() * -1;
                }
            }

            yield return null;
        }
        while (stackDir.Count != 0);

    }

    private bool CheckCondition(int index)
    {
        if ((pos + direction[index] * 2).x > width - 2) return false;
        if ((pos + direction[index] * 2).x < 0) return false;
        if ((pos + direction[index] * 2).y > height - 2) return false;
        if ((pos + direction[index] * 2).y < 0) return false;
        if (map[(pos + direction[index] * 2).x, (pos + direction[index] * 2).y] != (int)TILE.WALL) return false;
        return true;
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (map[x, y])
                    {
                        case (int)TILE.ROAD: Gizmos.color = Color.white; break;
                        case (int)TILE.WALL: Gizmos.color = Color.black; break;
                        case (int)TILE.CHECK: Gizmos.color = Color.yellow; break;
                        case (int)TILE.START: Gizmos.color = Color.red; break;
                    }
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
