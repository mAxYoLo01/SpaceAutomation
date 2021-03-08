using UnityEngine;

public class Entry : Block
{
    public string chosenItemId;

    public Entry(Vector2Int position) : base(position)
    {
        this.blockId = "entry";
    }

    public void SpawnItem()
    {
        if (chosenItemId == null)
            return;
        Item item = new Item(chosenItemId);
        item.GenerateItemUI();
        if (MoneyManager.instance.HasEnoughMoney(item.itemUI.price))
        {
            MoneyManager.instance.ChangeMoney(-item.itemUI.price);
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ItemOnConveyor"), GameObject.Find("ItemsOnConveyor").transform);
            go.GetComponent<ItemBehaviour>().Setup(this);
            go.GetComponent<SpriteRenderer>().sprite = item.itemUI.sprite;
            go.GetComponent<ItemBehaviour>().item = item;
            go.name = item.itemId + " " + item.id.ToString();
        }
    }

    public override GameObject OpenBlockGUI(Block block)
    {
        GameObject go = base.OpenBlockGUI(this);
        Transform entryItems = go.transform.Find("EntryItems");
        foreach (Transform child in entryItems)
            GameObject.Destroy(child.gameObject);
        foreach (ItemUI itemUI in Resources.LoadAll<ItemUI>("UIs/Items"))
        {
            if (itemUI.isInput)
            {
                GameObject entryItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/EntryItem"), entryItems);
                entryItem.GetComponent<EntryItemBehaviour>().SetupUI(itemUI, this);
                if (chosenItemId == itemUI.name)
                    entryItem.GetComponent<EntryItemBehaviour>().SelectItem();
            }
        }
        return go;
    }
}
