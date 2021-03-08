using UnityEngine;

public class Exit : Block
{

    public Exit(Vector2Int position) : base(position)
    {
        this.blockId = "exit";
    }

    public void SellItem(Item item)
    {
        SaveManager.instance.save.stats.AddItemToSold(item);
        MoneyManager.instance.ChangeMoney(item.itemUI.price);
    }
}
