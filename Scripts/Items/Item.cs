using System;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public string itemId;
    [NonSerialized]
    public ItemUI itemUI;

    public Item(string itemId)
    {
        this.itemId = itemId;
        this.id = Utils.GenerateRandomId();
        GenerateItemUI();
    }

    public void GenerateItemUI()
    {
        itemUI = Resources.Load<ItemUI>("UIs/Items/" + itemId);
    }

    public bool Equals(Item other)
    {
        return this.id == other.id;
    }
}
