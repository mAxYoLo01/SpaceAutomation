using UnityEngine;
using UnityEngine.UI;

public class EntryItemBehaviour : MonoBehaviour
{
    public string itemId;

    private Entry entry;

    public void SetupUI(ItemUI itemUI, Entry entry)
    {
        this.entry = entry;
        transform.Find("Selected").gameObject.SetActive(false);
        transform.Find("Icon").GetComponent<Image>().sprite = itemUI.sprite;
        itemId = itemUI.name;
        name = itemUI.name;
    }

    public void SelectItem()
    {
        foreach (Transform child in transform.parent)
            child.Find("Selected").gameObject.SetActive(false);
        transform.Find("Selected").gameObject.SetActive(true);
        entry.chosenItemId = itemId;
        SaveManager.instance.save.ModifyBlock(entry);
    }
}
