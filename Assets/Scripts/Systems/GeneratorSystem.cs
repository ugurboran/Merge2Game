// Assets/Scripts/Systems/GeneratorSystem.cs
using UnityEngine;
using System.Collections;

public class GeneratorSystem : MonoBehaviour
{
    public static GeneratorSystem Instance;
    void Awake() { Instance = this; }

    public void TryStartGenerator(ItemBase generatorItem)
    {
        var genSO = generatorItem.data as GeneratorSO;
        if (genSO == null) return;
        if (genSO.level < 5) return; // uretmez
        StartCoroutine(RunGenerator(generatorItem, genSO));
    }

    IEnumerator RunGenerator(ItemBase item, GeneratorSO genSO)
    {
        while (item != null && item.parentCell != null)
        {
            yield return new WaitForSeconds(genSO.spawnCooldown);
            // spawn attempt: spawn to an adjacent empty cell if exists; otherwise do nothing or try swap rule? Gorsellere gore board full uyarisi var
            var cell = item.parentCell;
            Cell target = FindEmptyAdjacent(cell);
            if (target == null)
            {
                // Board full davranisi: urunu olusturamayabilir, veya "Board Full!" UI göster.
                // continue to next interval
                continue;
            }
            // pick production based on probabilities
            ItemSO chosen = PickByProbability(genSO.produces, genSO.probabilities);
            var go = Instantiate(chosen.prefab);
            var product = go.GetComponent<ItemBase>();
            product.InitializeFromSO(chosen);
            target.PlaceItem(product);
        }
    }

    Cell FindEmptyAdjacent(Cell origin)
    {
        GridManager gm = FindObjectOfType<GridManager>();
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, 1, 0, -1 };
        foreach (var dir in new int[][] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { 0, 1 } })
        {
            int nx = origin.x + dir[0], ny = origin.y + dir[1];
            if (nx >= 0 && nx < gm.width && ny >= 0 && ny < gm.height)
            {
                var c = gm.cells[nx, ny];
                if (c.IsEmpty) return c;
            }
        }
        return null;
    }

    ItemSO PickByProbability(ItemSO[] arr, float[] probs)
    {
        float r = Random.value;
        float sum = 0f;
        for (int i = 0; i < probs.Length; i++)
        {
            sum += probs[i];
            if (r <= sum) return arr[i];
        }
        return arr[arr.Length - 1];
    }
}
