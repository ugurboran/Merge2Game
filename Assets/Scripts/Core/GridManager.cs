// Assets/Scripts/Core/GridManager.cs
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public int width = 5, height = 5;
    public GameObject cellPrefab;
    public Transform cellParent;
    public Cell[,] cells;

    // initial placement SO refereces:
    public ItemSO startingGeneratorSO; // G1_1

    void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var go = Instantiate(cellPrefab, cellParent);
                go.name = $"Cell_{x}_{y}";
                var c = go.GetComponent<Cell>();
                c.x = x; c.y = y;
                cells[x, y] = c;
            }
        }
        // center placement: orn orta hucre
        int cx = width / 2, cy = height / 2;
        // spawn starting generator prefab:
        if (startingGeneratorSO != null)
        {
            var itemGO = Instantiate(startingGeneratorSO.prefab);
            var item = itemGO.GetComponent<ItemBase>();
            item.InitializeFromSO(startingGeneratorSO);
            cells[cx, cy].PlaceItem(item);
        }
    }

    public Cell GetCellAtWorldPos(Vector2 worldPos)
    {
        // basit: en yakin hucreyi hesapla (projeye gore raycast kullan)
        Cell nearest = null;
        float best = float.MaxValue;
        foreach (var c in cells)
        {
            float d = Vector2.SqrMagnitude((Vector2)c.transform.position - worldPos);
            if (d < best) { best = d; nearest = c; }
        }
        return nearest;
    }
}
