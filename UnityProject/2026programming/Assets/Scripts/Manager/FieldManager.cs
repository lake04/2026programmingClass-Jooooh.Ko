using UnityEngine;
using System.Collections.Generic;

public struct Grid
{
    public int cost;
    public Vector2 direction;
}

public class FieldManager : ManagerBase
{
    public static FieldManager Instance;

    [Header("그리드 설정")]
    [SerializeField] private int gridWidth = 30;
    [SerializeField] private int gridHeight = 20;
    [SerializeField] private float cellSize = 1.2f;

    public Transform player;

    private Grid[,] grid;
    private Vector2 gridOrigin;
    private Queue<Vector2Int> searchQueue = new Queue<Vector2Int>(600);

    public override void Init()
    {
        Instance = this;
        grid = new Grid[gridWidth, gridHeight];
        player = GameManager.Instance.player.transform;
    }

    void Update()
    {
        if(player !=null && Time.frameCount % 10 ==0)
        {
            UpdateFlowFied();
        }
    }

    private void UpdateFlowFied()
    {
        gridOrigin = (Vector2)player.position - new Vector2(gridWidth * cellSize * 0.5f,gridHeight * cellSize * 0.5f);

        for(int x=0; x <gridWidth; x++)
        {
            for(int y=0; y<gridHeight; y++)
            {
                grid[x,y].cost = int.MaxValue;
                grid[x,y].direction = Vector2.zero;
            }
        }

        Vector2Int playerGridPos = WorldToGrid(player.position);

        if (IsInsideGrid(playerGridPos.x, playerGridPos.y))
        {
            grid[playerGridPos.x, playerGridPos.y].cost = 0;
            searchQueue.Clear();
            searchQueue.Enqueue(playerGridPos);

            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            while (searchQueue.Count > 0)
            {
                Vector2Int cur = searchQueue.Dequeue();
                int nextCost = grid[cur.x, cur.y].cost + 1;

                for (int i = 0; i < 4; i++)
                {
                    int nx = cur.x + dx[i];
                    int ny = cur.y + dy[i];
                    if (IsInsideGrid(nx, ny) && grid[nx, ny].cost == int.MaxValue)
                    {
                        grid[nx, ny].cost = nextCost;
                        searchQueue.Enqueue(new Vector2Int(nx, ny));
                    }
                }
            }

            for(int x= 0; x < gridWidth; x++)
            {
                for(int y=0; y< gridHeight; y++)
                {
                    grid[x,y].direction = GetBaseDirection(x,y);
                }
            }
        }
    }
    
    private Vector2 GetBaseDirection(int x, int y)
    {
        int minCost = grid[x,y].cost;
        Vector2Int baseMove = new Vector2Int(x,y);

        for(int dx  = -1; dx <= 1; dx++)
        {
            for(int dy = -1; dy <= 1; dy++)
            {
              if(dx == 0 && dy ==0)
                {
                    continue;
                }

                int nx = x + dx;
                int ny = y + dy;

                if(IsInsideGrid(nx, ny) && grid[nx,ny].cost < minCost)
                {
                    minCost = grid[nx, ny].cost;
                    baseMove = new Vector2Int(nx, ny);
                }
            }
        }

        if(baseMove.x == x && baseMove.y == y)
        {
            return Vector2.zero;
        }
        return new Vector2(baseMove.x - x, baseMove.y - y).normalized;
    }

    public Vector2 GetDirection(Vector3 worldPos)
    {
        Vector2Int gPos = WorldToGrid(worldPos);
        if(IsInsideGrid(gPos.x,gPos.y))
        {
            return grid[gPos.x, gPos.y].direction;
        }

        return ((Vector2)player.position - (Vector2)worldPos).normalized;
    }

    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / cellSize);
        return new Vector2Int(x, y);
    }

    private bool IsInsideGrid(int x, int y) => x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;

}
