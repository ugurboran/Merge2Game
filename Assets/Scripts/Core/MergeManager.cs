// Assets/Scripts/Core/MergeManager.cs
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;
    public ItemSO[] allSO; // tum so assetleri, id uzerinden lookup

    void Awake() { Instance = this; }

    public void TryMerge(ItemBase a, ItemBase b)
    {
        // yalnizca ayni id ve ayni family/level ise
        if (a.data.id != b.data.id)
        {
            // burada zaten swap mantigi ItemBase'te yapildigi icin gelmeyecek genelde
            return;
        }

        // hesapla yeni level id: örn "G1_1" -> "G1_2"
        string newId = NextId(a.data.id);
        ItemSO newSO = FindSOById(newId);
        if (newSO == null)
        {
            Debug.LogWarning("No SO for merge result: " + newId);
            return;
        }

        var parent = b.parentCell; // b'nin oldugu hucrede yeni olustur
        // remove both
        a.parentCell.RemoveItem(); Destroy(a.gameObject);
        b.parentCell.RemoveItem(); Destroy(b.gameObject);

        // spawn new item
        var go = Instantiate(newSO.prefab);
        var item = go.GetComponent<ItemBase>();
        item.InitializeFromSO(newSO);
        parent.PlaceItem(item);

        // special: if new item is generator and level >=5, enable generation behavior via GeneratorSystem
        if (newSO is GeneratorSO)
        {
            // but we instantiate correct type etc. (see below)
        }
    }

    string NextId(string id)
    {
        // orn "G1_1" split
        var parts = id.Split('_');
        if (parts.Length != 2) return id;
        int lvl = int.Parse(parts[1]);
        lvl++;
        return $"{parts[0]}_{lvl}";
    }

    ItemSO FindSOById(string id)
    {
        foreach (var s in allSO) if (s.id == id) return s;
        return null;
    }
}
