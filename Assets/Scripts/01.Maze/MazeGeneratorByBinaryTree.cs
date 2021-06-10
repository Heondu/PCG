using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGeneratorByBinaryTree : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    private int[,] map;

    private const int ROAD = 0;
    private const int WALL = 1;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;

    [SerializeField] private Color[] colors;

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
                OnDrawTile(x, y); //맵 크기에 맞춰 타일맵 배치
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 1 && y == 0) map[x, y] = ROAD; //시작 위치
                else if (x == width - 2 && y == height - 1) map[x, y] = ROAD; //출구 위치
                else if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = WALL; //가장자리 벽으로 채움
                else if (x % 2 == 0 || y % 2 == 0) map[x, y] = WALL; //짝수 칸 벽으로 채움
                else map[x, y] = ROAD; //나머지 칸에는 길 배치
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos;
                if (x % 2 == 0 || y % 2 == 0) continue; //짝수 칸은 건너 뜀
                if (x == width - 2 && y == height - 2) continue; //우측 상단 모서리에 닿으면 길을 생성하지 않음
                if (x == width - 2) pos = new Vector2Int(x, y + 1); //오른쪽 끝에 닿으면 길 생성 방향을 위로 설정
                else if (y == height - 2) pos = new Vector2Int(x + 1, y); //위쪽 끝에 닿으면 길 생성 방향을 오른쪽으로 설정
                else if (Random.Range(0, 2) == 0) pos = new Vector2Int(x + 1, y); //랜덤으로 방향 지정 (위쪽, 오른쪽)
                else pos = new Vector2Int(x, y + 1);
                map[pos.x, pos.y] = ROAD; //맵 데이터에 값 저장
                SetTileColor(pos.x, pos.y); //타일맵 색상 변경
            }
        }
    }

    private void SetTileColor(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0); //생성 위치를 화면 중앙으로 설정
        tilemap.SetTileFlags(pos, TileFlags.None); //타일맵의 색상을 변경하기 위해 TileFlags값을 None으로 변경
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