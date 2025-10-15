using UnityEngine;

/// <summary>
/// Generates a grid layout and instantiates cell prefabs on the scene.
/// </summary>
public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [Tooltip("Number of rows and columns (e.g., 5x5)")]
    public int gridSize = 5;

    [Tooltip("Distance between each cell")]
    public float cellSpacing = 1.1f;

    [Tooltip("The prefab used for each cell")]
    public GameObject cellPrefab;

    private Cell[,] gridArray;

    void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// Creates the grid and instantiates each cell.
    /// </summary>
    private void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("GridManager: Cell Prefab is not assigned!");
            return;
        }

        gridArray = new Cell[gridSize, gridSize];

        // Offset to center the grid on screen
        Vector3 startPos = transform.position - new Vector3((gridSize - 1) * cellSpacing / 2f, (gridSize - 1) * cellSpacing / 2f, 0);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector3 pos = startPos + new Vector3(x * cellSpacing, y * cellSpacing, 0);
                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cellObj.name = $"Cell_{x}_{y}";

                Cell cell = cellObj.GetComponent<Cell>();
                cell.gridPosition = new Vector2Int(x, y);
                gridArray[x, y] = cell;
            }
        }

        Debug.Log($"{gridSize}x{gridSize} grid created ({gridSize * gridSize} cells total).");
    }

    /// <summary>
    /// Returns a cell by grid coordinates.
    /// </summary>
    public Cell GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridSize || y >= gridSize)
            return null;
        return gridArray[x, y];
    }
}
