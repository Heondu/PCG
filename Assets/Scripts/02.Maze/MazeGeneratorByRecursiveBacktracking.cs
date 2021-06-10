using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum TILE {ROAD = 0, WALL, CHECK, START};

public class MazeGeneratorByRecursiveBacktracking : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    private int[,] map;

    private Vector2Int[] direction = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private Vector2Int pos = Vector2Int.zero;
    private Stack<Vector2Int> stackDir = new Stack<Vector2Int>(); //지나온 길의 방향 저장

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;

    [SerializeField] private Color[] colors; //타일 색 저장

    private void Update()
    {
        Debug.Assert(!(width == 0 || height == 0), "크기를 입력하십시오.");
        if (Input.GetMouseButtonDown(0)) Generate();
    }

    public void Generate()
    {
        Init(); //미로 초기화
        RandPosSelect(); //시작 지점 무작위 선택
        StartCoroutine(GenerateRoad()); //미로 생성
    }

    private void Init()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (int)TILE.WALL; //모든 타일을 벽으로 채움
                OnDrawTile(x, y); //타일 생성
                SetTileColor(x, y); //타일 색상 설정
            }
        }
    }

    private void RandPosSelect()
    {
        do
        {
            pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height)); //미로 범위 내에서 무작위 선택
        } while (pos.x % 2 == 0 || pos.y % 2 == 0); //홀수 칸으로 설정
    }

    private void RandDirection() //무작위로 방향을 섞는 메소드
    {
        for (int i = 0; i < direction.Length; i++)
        {
            int randNum = Random.Range(0, direction.Length); //4방향 중 무작위로 방향 선택
            Vector2Int temp = direction[randNum]; //현재 인덱스에 해당하는 방향과 랜덤으로 선택한 방향을 서로 바꿈
            direction[randNum] = direction[i];
            direction[i] = temp;
        }
    }

    private IEnumerator GenerateRoad()
    {
        map[pos.x, pos.y] = (int)TILE.START; //RandPosSelect 함수에서 무작위로 선택한 지점을 시작 지점으로 설정
        SetTileColor(pos.x, pos.y);
        do
        {
            int index = -1; //-1은 갈 수 있는 길이 없음을 의미

            RandDirection(); //방향 무작위로 섞음

            for (int i = 0; i < direction.Length; i++)
            {
                if (CheckForRoad(i))
                {
                    index = i; //선택한 방향에 길이 없을 경우 방향 배열의 인덱스 저장
                    break;
                }
            }

            if (index != -1) //갈 수 있는 길이 있을 경우
            {
                for (int i = 0; i < 2; i++) //길과 길 사이에 벽을 생성하기 위해 3칸을 길로 바꿈
                {
                    stackDir.Push(direction[index]); //스택에 방향 저장
                    pos += direction[index]; //위치 변수 수정
                    map[pos.x, pos.y] = (int)TILE.CHECK; //타일 생성
                    SetTileColor(pos.x, pos.y); //타일 색상 변경
                }
            }
            else //갈 수 있는 길이 없을 경우
            {
                for (int i = 0; i < 2; i++) //길을 만들 때 3칸을 만들었기 때문에 뒤로 돌아갈 때도 3칸 이동
                {
                    map[pos.x, pos.y] = (int)TILE.ROAD; //완성된 길 의미
                    SetTileColor(pos.x, pos.y);
                    pos += stackDir.Pop() * -1; //방향을 저장하는 스택에서 데이터를 꺼낸뒤 -1을 곱해 방향 반전
                }
            }

            yield return null;
        }
        while (stackDir.Count != 0); //스택이 0이라는 것은 모든 길을 순회했다는 것을 의미하므로 반복문 종료

    }

    private bool CheckForRoad(int index)
    {
        if ((pos + direction[index] * 2).x > width - 2) return false; //2를 곱하는 이유는 길과 길 사이에 벽이 있기 때문
        if ((pos + direction[index] * 2).x < 0) return false;
        if ((pos + direction[index] * 2).y > height - 2) return false;
        if ((pos + direction[index] * 2).y < 0) return false;
        if (map[(pos + direction[index] * 2).x, (pos + direction[index] * 2).y] != (int)TILE.WALL) return false;
        return true;
    }

    private void SetTileColor(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0); //미로의 위치를 카메라 중앙으로 맞춤
        tilemap.SetTileFlags(pos, TileFlags.None); //타일 색상을 변경하기 위해 TileFlags를 None으로 설정
        switch (map[x, y])
        {
            case (int)TILE.ROAD: tilemap.SetColor(pos, colors[0]); break;
            case (int)TILE.WALL: tilemap.SetColor(pos, colors[1]); break;
            case (int)TILE.CHECK: tilemap.SetColor(pos, colors[2]); break;
            case (int)TILE.START: tilemap.SetColor(pos, colors[3]); break;
        }
    }

    private void OnDrawTile(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
        tilemap.SetTile(pos, tile); //타일 생성
    }
}