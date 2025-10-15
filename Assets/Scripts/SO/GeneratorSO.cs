// Assets/Scripts/SO/GeneratorSO.cs
using UnityEngine;

[CreateAssetMenu(menuName = "MergeGame/GeneratorSO")]
public class GeneratorSO : ItemSO
{
    public bool isGenerator; // true
    // uretim sirasinda kullanmak uzere tercihler listesi:
    public ItemSO[] produces; // olasi uretimler (P1_1,P1_2,P2_1 vb)
    public float[] probabilities; // toplam 1.0 olmali (or: 0.86,0.08,0.06)
    public float spawnCooldown = 2f;
}
