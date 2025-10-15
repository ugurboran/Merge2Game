// Assets/Scripts/Core/Cell.cs
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public ItemBase currentItem;

    public bool IsEmpty => currentItem == null;

    public void PlaceItem(ItemBase item)
    {
        if (item == null) return;
        currentItem = item;
        item.SetParentCell(this);
        item.transform.position = transform.position;
    }

    public ItemBase RemoveItem()
    {
        var temp = currentItem;
        currentItem = null;
        return temp;
    }
}
