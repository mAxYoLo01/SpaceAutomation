using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public GameObject delete;
    public GameObject cancel;
    public GameObject rotate;
    public GameObject upgrade;
    public GameObject validate;
    public GameObject buildMenu;
    public Transform scrollParent;
    public Transform blockGUIs;

    public static BuildManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BuildManager found!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        foreach (Transform child in scrollParent)
            Destroy(child.gameObject);
        foreach (BlockUI blockUI in Resources.LoadAll<BlockUI>("UIs/Blocks"))
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/MenuBlock"), scrollParent);
            go.GetComponent<MenuBlockBehaviour>().SetupUI(blockUI);
        }
    }

    public void OpenMainView()
    {
        OpenCloseEditOptions(false);
        OpenCloseBuildOptions(false);
        if (CameraMovement.instance.selectedObject != null)
        {
            CameraMovement.instance.selectedObject.GetComponent<SpriteRenderer>().color = Color.white;
            CameraMovement.instance.selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (TabManager.instance.currentTab == Tab.BUILD)
                Destroy(CameraMovement.instance.selectedObject);
            CameraMovement.instance.selectedObject = null;
        }
    }

    public void OpenBuildView()
    {
        CameraMovement.instance.inMenu = true;
        OpenCloseEditOptions(false);
        if (CameraMovement.instance.selectedObject != null)
        {
            CameraMovement.instance.selectedObject.GetComponent<SpriteRenderer>().color = Color.white;
            CameraMovement.instance.selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            CameraMovement.instance.selectedObject = null;
        }
        buildMenu.SetActive(true);
    }

    public void OpenEditView()
    {

    }

    public void DeleteBlock()
    {
        Destroy(CameraMovement.instance.selectedObject);
        SaveManager.instance.save.DeleteBlock(CameraMovement.instance.selectedObject.GetComponent<BlockBehaviour>().block);
        CameraMovement.instance.selectedObject = null;
        OpenCloseEditOptions(false);
    }

    public void ValidateBuild()
    {
        GameObject newGo = CameraMovement.instance.selectedObject;
        newGo.GetComponent<SpriteRenderer>().color = Color.white;
        newGo.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Block block = newGo.GetComponent<BlockBehaviour>().block;
        block.position = new Vector2Int(Mathf.FloorToInt(newGo.transform.position.x), Mathf.FloorToInt(newGo.transform.position.y));
        if (TabManager.instance.currentTab == Tab.BUILD)
        {
            if (MoneyManager.instance.HasEnoughMoney(block.blockUI.cost))
            {
                newGo.transform.SetParent(GameObject.Find("Blocks").transform);
                MoneyManager.instance.ChangeMoney(-block.blockUI.cost);
                SaveManager.instance.save.AddBlock(block);
            }
            else
            {
                Destroy(newGo);
                print("Not enough money for " + block.blockId + "!");
            }
            OpenCloseBuildOptions(false);
            TabManager.instance.currentTab = Tab.MAIN;
        }
        else if (TabManager.instance.currentTab == Tab.EDIT)
        {
            OpenCloseEditOptions(false);
            SaveManager.instance.save.ModifyBlock(block);
        }
        CameraMovement.instance.selectedObject = null;
    }

    public void ChangeBuildOrientation()
    {
        Block block = CameraMovement.instance.selectedObject.GetComponent<BlockBehaviour>().block;
        block.orientation = (block.orientation + 1) % 4;
        CameraMovement.instance.selectedObject.transform.rotation = Quaternion.Euler(0, 0, block.orientation * 90f);
        if (Utils.IsPositionValid(block.position, block))
        {
            validate.GetComponent<Button>().interactable = true;
        }
        else
        {
            validate.GetComponent<Button>().interactable = false;
        }
    }

    public void CancelBuild()
    {
        Destroy(CameraMovement.instance.selectedObject);
        OpenCloseBuildOptions(false);
        CameraMovement.instance.selectedObject = null;
        TabManager.instance.SwitchToTab((int)Tab.MAIN);
    }

    public void OpenCloseBuildOptions(bool active)
    {
        cancel.SetActive(active);
        rotate.SetActive(active);
        validate.SetActive(active);
    }

    public void OpenCloseEditOptions(bool active)
    {
        delete.SetActive(active);
        upgrade.SetActive(active);
        rotate.SetActive(active);
        validate.SetActive(active);
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
        CameraMovement.instance.inMenu = false;
        TabManager.instance.currentTab = Tab.MAIN;
    }

    public void CloseBlockGUI()
    {
        foreach (Transform child in blockGUIs)
            child.gameObject.SetActive(false);
        CameraMovement.instance.inMenu = false;
    }
}
