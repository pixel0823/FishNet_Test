using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<BaseItem> items = new();
    private Dictionary<string, BaseItem> map;

    public void Build()
    {
        map = new Dictionary<string, BaseItem>();
        foreach (var it in items)
        {
            if (it == null || string.IsNullOrEmpty(it.itemId)) continue;
            if (!map.ContainsKey(it.itemId)) map.Add(it.itemId, it);
            else Debug.LogWarning($"Duplicate Item ID: {it.itemId}");
        }
    }

    public BaseItem GetById(string id)
    {
        if (map == null) Build();
        return map != null && map.TryGetValue(id, out var v) ? v : null;
    }
}
