using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneratorByBinarySpacePartitioning
{
    public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public RectInt treeSize;
        public RectInt dungeonSize;

        public TreeNode(int x, int y, int width, int height)
        {
            treeSize.x = x;
            treeSize.y = y;
            treeSize.width = width;
            treeSize.height = height;
        }
    }

    public class DungeonGeneratorByBinarySpacePartitioning : MonoBehaviour
    {
        [SerializeField] private Vector2Int mapSize;
        [SerializeField] private int maxNode;
        [SerializeField] private float minDivideSize;
        [SerializeField] private float maxDivideSize;
        [SerializeField] private int minRoomSize;
        [SerializeField] private GameObject line;
        [SerializeField] private Transform lineHolder;
        [SerializeField] private GameObject rectangle;
        [SerializeField] private Tile tile;
        [SerializeField] private Tilemap tilemap;
        private TreeNode rootNode;

        private void Awake()
        {
            OnDrawRectangle(0, 0, mapSize.x, mapSize.y);
            rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y);
        }

        private void DivideTree(TreeNode treeNode, int n)
        {
            if (n < maxNode)
            {
                RectInt size = treeNode.treeSize;
                int length = (size.width >= size.height ? size.width : size.height);
                int split = Mathf.RoundToInt(Random.Range(length * minDivideSize, length * maxDivideSize));
                if (size.width >= size.height)
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height);
                    treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height);
                    OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height));
                }
                else
                {
                    treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split);
                    treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                    OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
                }
                treeNode.leftTree.parentTree = treeNode;
                treeNode.rightTree.parentTree = treeNode;
                DivideTree(treeNode.leftTree, n + 1);
                DivideTree(treeNode.rightTree, n + 1);
            }
        }

        private RectInt GenerateDungeon(TreeNode treeNode, int n)
        {
            if (n == maxNode)
            {
                RectInt size = treeNode.treeSize;
                int width = Mathf.Max(Random.Range(size.width / 2, size.width - 1));
                int height = Mathf.Max(Random.Range(size.height / 2, size.height - 1));
                int x = treeNode.treeSize.x + Random.Range(1, size.width - width);
                int y = treeNode.treeSize.y + Random.Range(1, size.height - height);
                OnDrawDungeon(x, y, width, height);
                return new RectInt(x, y, width, height);
            }
            treeNode.leftTree.dungeonSize = GenerateDungeon(treeNode.leftTree, n + 1);
            treeNode.rightTree.dungeonSize = GenerateDungeon(treeNode.rightTree, n + 1);
            return treeNode.leftTree.dungeonSize;
        }

        private void GenerateRoad(TreeNode treeNode, int n)
        {
            if (n == maxNode) return;
            int x1 = GetCenterX(treeNode.leftTree.dungeonSize);
            int x2 = GetCenterX(treeNode.rightTree.dungeonSize);
            int y1 = GetCenterY(treeNode.leftTree.dungeonSize);
            int y2 = GetCenterY(treeNode.rightTree.dungeonSize);
            for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
                tilemap.SetTile(new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0), tile);
            for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
                tilemap.SetTile(new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0), tile);
            GenerateRoad(treeNode.leftTree, n + 1);
            GenerateRoad(treeNode.rightTree, n + 1);
        }

        private void OnDrawLine(Vector2 from, Vector2 to)
        {
            LineRenderer lineRenderer = Instantiate(line, lineHolder).GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, from - mapSize / 2);
            lineRenderer.SetPosition(1, to - mapSize / 2);
        }

        private void OnDrawDungeon(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
                for (int j = y; j < y + height; j++)
                    tilemap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), tile);
        }

        private void OnDrawRectangle(int x, int y, int width, int height)
        {
            LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2);
            lineRenderer.SetPosition(1, new Vector2(x + width, y) - mapSize / 2);
            lineRenderer.SetPosition(2, new Vector2(x + width, y + height) - mapSize / 2);
            lineRenderer.SetPosition(3, new Vector2(x, y + height) - mapSize / 2);
        }

        private int GetCenterX(RectInt size)
        {
            return size.x + size.width / 2;
        }

        private int GetCenterY(RectInt size)
        {
            return size.y + size.height / 2;
        }

        public void OnDivide()
        {
            DivideTree(rootNode, 0);
        }

        public void OnGenerateDungeon()
        {
            GenerateDungeon(rootNode, 0);
        }

        public void OnGenerateRoad()
        {
            GenerateRoad(rootNode, 0);
        }
    }
}
