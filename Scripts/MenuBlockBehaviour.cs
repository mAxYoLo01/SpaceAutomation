using UnityEngine;
using UnityEngine.UI;

public class MenuBlockBehaviour : MonoBehaviour
{
    public Image blockImage;
    public Text blockName;

    public string blockId;

    public void SelectBlock()
    {
        BuildManager.instance.OpenCloseBuildOptions(true);
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3Int pos2 = MapGenerator.instance.tileMap.WorldToCell(new Vector3(Mathf.Clamp(pos.x, 0, MapGenerator.instance.width - 1), Mathf.Clamp(pos.y, 0, MapGenerator.instance.height - 1), 0));
        Block newBlock = Utils.GetBlockFromId(blockId, (Vector2Int)pos2);
        GameObject newGo = MapGenerator.instance.CreateBlockUI(newBlock);
        newGo.GetComponent<SpriteRenderer>().sortingOrder = 2;
        newGo.GetComponent<SpriteRenderer>().color = Color.yellow;
        newGo.transform.SetParent(GameObject.Find("BlocksBeingBuilt").transform);
        CameraMovement.instance.selectedObject = newGo;
        BuildManager.instance.CloseBuildMenu();
        TabManager.instance.currentTab = Tab.BUILD;
    }

    public void SetupUI(BlockUI blockUI)
    {
        blockId = blockUI.name;
        blockName.text = blockUI.name;
        blockImage.sprite = blockUI.sprite;
        name = blockUI.name;
    }
}
