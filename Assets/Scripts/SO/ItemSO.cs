// Assets/Scripts/SO/ItemSO.cs
using UnityEngine;

public enum ItemFamily { G1, P1, P2 }

[CreateAssetMenu(menuName = "MergeGame/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string id; // orn "G1_1"
    public ItemFamily family;
    public int level; // 1..10
    public Sprite sprite;
    public GameObject prefab; // gorsel prefab referansi (opsiyonel)
}
