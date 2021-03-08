using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Block
{
    public List<CraftItem> inventory = new List<CraftItem>();
    [NonSerialized]
    public MachineUI machineUI;
    public string craftChosen;

    public Machine(Vector2Int position) : base(position)
    {

    }

    public void GenerateMachineUI()
    {
        machineUI = Resources.Load<MachineUI>("UIs/Machines/" + blockId);
    }

    public bool AddItemInInventory(Item item)
    {
        foreach (CraftItem craftItem in inventory)
        {
            if (craftItem.itemId == item.itemId)
            {
                if (craftItem.number >= 5)
                {
                    Debug.Log($"Could not add {item.itemId}");
                    return false;
                }
                craftItem.number++;
                SaveManager.instance.save.ModifyBlock(this);
                return true;
            }
        }
        inventory.Add(new CraftItem(1, item.itemId));
        SaveManager.instance.save.ModifyBlock(this);
        return true;
    }

    public override GameObject OpenBlockGUI(Block block)
    {
        GameObject go = base.OpenBlockGUI(this);
        Transform craftItems = go.transform.Find("CraftItems");
        foreach (Transform child in craftItems)
            GameObject.Destroy(child.gameObject);
        foreach (CraftUI craftUI in Resources.LoadAll<CraftUI>("UIs/Crafts/" + blockId))
        {
            GameObject craftItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/CraftItem"), craftItems);
            craftItem.GetComponent<CraftItemBehaviour>().SetupUI(craftUI, this);
            if (craftChosen == craftUI.name)
                craftItem.GetComponent<CraftItemBehaviour>().SelectItem();
        }
        return go;
    }

    public IEnumerator TryCraft()
    {
        CraftUI craft = GetCraft();
        if (craft != null)
        {
            yield return new WaitForSeconds(craft.speed);
            BuildManager.instance.StartCoroutine(Craft(craft));
        }
    }

    public CraftUI GetCraft()
    {
        if (craftChosen != null)
        {
            CraftUI craft = Resources.Load<CraftUI>($"UIs/Crafts/{blockId}/{craftChosen}");
            if (CheckIfCraftable(craft))
            {
                return craft;
            }
        }
        return null;
    }

    public bool CheckIfCraftable(CraftUI craft)
    {
        foreach (CraftItem inputItem in craft.inputs)
        {
            bool itemPresent = false;
            foreach (CraftItem inventoryItem in inventory)
            {
                if (inventoryItem.itemId == inputItem.itemId && inventoryItem.number >= inputItem.number)
                {
                    itemPresent = true;
                    break;
                }
            }
            if (!itemPresent)
                return false;
        }
        return true;
    }

    public IEnumerator Craft(CraftUI craft)
    {
        foreach (CraftItem inputItem in craft.inputs)
        {
            foreach (CraftItem inventoryItem in inventory)
            {
                if (inventoryItem.itemId == inputItem.itemId)
                {
                    for (int i = 0; i < inputItem.number; i++)
                    {
                        inventoryItem.number--;
                    }
                    if (inventoryItem.number == 0)
                    {
                        inventory.Remove(inventoryItem);
                    }
                    break;
                }
            }
        }
        SaveManager.instance.save.ModifyBlock(this);
        foreach (CraftItem item in craft.outputs)
        {
            for (int i = 0; i < item.number; i++)
            {
                Item spawnedItem = new Item(item.itemId);
                SaveManager.instance.save.stats.AddItemToCrafted(spawnedItem);
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ItemOnConveyor"), GameObject.Find("ItemsOnConveyor").transform);
                go.GetComponent<ItemBehaviour>().Setup(this);
                go.GetComponent<SpriteRenderer>().sprite = spawnedItem.itemUI.sprite;
                go.GetComponent<ItemBehaviour>().item = spawnedItem;
                go.name = spawnedItem.itemId + " " + spawnedItem.id.ToString();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

