// Assets/Scripts/Core/ItemBase.cs
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSO data;
    protected GridManager grid;
    public Cell parentCell;
    Vector3 startPos;
    CanvasGroup canvasGroup;

    public virtual void Awake()
    {
        grid = FindObjectOfType<GridManager>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void InitializeFromSO(ItemSO so)
    {
        data = so;
        // set sprite, visuals...
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && so.sprite != null) sr.sprite = so.sprite;
    }

    public void SetParentCell(Cell c)
    {
        parentCell = c;
        transform.position = c.transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(eventData.position);
        wp.z = 0;
        transform.position = wp;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Vector3 wp = Camera.main.ScreenToWorldPoint(eventData.position);
        wp.z = 0;
        var targetCell = grid.GetCellAtWorldPos(wp);
        if (targetCell == null)
        {
            // geri doner
            transform.position = startPos;
            return;
        }

        // if same item clicked on same-lvl -> merge
        if (!targetCell.IsEmpty && targetCell.currentItem.data.id == data.id)
        {
            // ask MergeManager
            MergeManager.Instance.TryMerge(this, targetCell.currentItem);
            return;
        }

        // if dropped on a different item -> swap places (herhangi bir item kendisiyle ayni item uzerine birakilmazsa, uzerine birakildigi item ile yer degistirir)
        if (!targetCell.IsEmpty && targetCell.currentItem.data.id != data.id)
        {
            var other = targetCell.currentItem;
            var oldCell = parentCell;
            // swap
            oldCell.RemoveItem();
            targetCell.RemoveItem();
            oldCell.PlaceItem(other);
            targetCell.PlaceItem(this);
            return;
        }

        // if empty -> place
        if (targetCell.IsEmpty)
        {
            parentCell.RemoveItem();
            targetCell.PlaceItem(this);
            return;
        }

        // fallback
        transform.position = startPos;
    }
}
