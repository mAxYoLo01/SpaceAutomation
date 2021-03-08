using UnityEngine;
using UnityEngine.UI;

public class CraftItemBehaviour : MonoBehaviour
{
    public Transform inputs;
    public Transform outputs;
    public string craftId;

    private Machine machine;

    public void SetupUI(CraftUI craftUI, Machine machine)
    {
        this.machine = machine;
        craftId = craftUI.name;
        name = craftUI.name;
        transform.Find("Selected").gameObject.SetActive(false);
        foreach (Transform child in inputs)
            Destroy(child.gameObject);
        foreach (CraftItem item in craftUI.inputs)
        {
            GameObject itemGo = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SingleCraftItem"), inputs);
            itemGo.GetComponent<Image>().sprite = Resources.Load<ItemUI>("UIs/Items/" + item.itemId).sprite;
            itemGo.GetComponentInChildren<Text>().text = item.number.ToString();
        }
        foreach (Transform child in outputs)
            Destroy(child.gameObject);
        foreach (CraftItem item in craftUI.outputs)
        {
            GameObject itemGo = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SingleCraftItem"), outputs);
            itemGo.GetComponent<Image>().sprite = Resources.Load<ItemUI>("UIs/Items/" + item.itemId).sprite;
            itemGo.GetComponentInChildren<Text>().text = item.number.ToString();
        }
        if (!craftUI.AreRequirementsMet())
        {
            transform.Find("Locked").gameObject.SetActive(true);
        }
    }

    public void SelectItem()
    {
        if (Resources.Load<CraftUI>($"UIs/Crafts/{machine.blockId}/{craftId}").AreRequirementsMet())
        {
            foreach (Transform child in transform.parent)
                child.Find("Selected").gameObject.SetActive(false);
            transform.Find("Selected").gameObject.SetActive(true);
            machine.craftChosen = craftId;
            SaveManager.instance.save.ModifyBlock(machine);
        }
    }
}
