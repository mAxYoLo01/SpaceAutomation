using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CraftUI", menuName = "CraftUI")]
public class CraftUI : ScriptableObject
{
    public List<CraftItem> inputs;
    public List<CraftItem> outputs;
    public float speed;
    public List<Requirement> requirements;

    public bool AreRequirementsMet()
    {
        foreach (Requirement requirement in requirements)
        {
            List<CraftItem> items = new List<CraftItem>();
            if (requirement.type == RequirementType.ITEMS_CRAFTED)
                items = SaveManager.instance.save.stats.crafted;
            else if (requirement.type == RequirementType.ITEMS_SOLD)
                items = SaveManager.instance.save.stats.sold;
            CraftItem item = items.Find(_item => _item.itemId == requirement.item.itemId);
            if (item == null || item.number < requirement.item.number)
                return false;
        }
        return true;
    }
}

[Serializable]
public class CraftItem
{
    public int number;
    public string itemId;

    public CraftItem(int number, string itemId)
    {
        this.number = number;
        this.itemId = itemId;
    }
}
