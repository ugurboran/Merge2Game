using UnityEngine;

/// <summary>
/// Represents a single cell on the grid.
/// Currently holds its position and occupancy status.
/// </summary>
public class Cell : MonoBehaviour
{
    public Vector2Int gridPosition; // Position in the grid
    public bool isOccupied = false; // Whether this cell currently has an item

    // The item currently placed on this cell (if any)
    public GameObject currentItem; // Items will be added
}
