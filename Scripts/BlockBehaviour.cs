using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviour : MonoBehaviour
{
    public Block block;

    private void OnMouseDown()
    {
        if (CameraMovement.instance.selectedObject == gameObject)
        {
            CameraMovement.instance.draggingBlock = true;
        }
    }

    private void OnMouseDrag()
    {
        if (CameraMovement.instance.selectedObject == gameObject)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int pos2 = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
            transform.position = new Vector3(pos2.x + 0.5f, pos2.y + 0.5f, 0);
            if (Utils.IsPositionValid(pos2, block))
            {
                BuildManager.instance.validate.GetComponent<Button>().interactable = true;
                GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                BuildManager.instance.validate.GetComponent<Button>().interactable = false;
                GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void OnMouseUp()
    {
        if (TabManager.instance.currentTab == Tab.MAIN && !CameraMovement.instance.inMenu && !CameraMovement.instance.draggingBlock)
        {
            block.OpenBlockGUI(block);
        }
        CameraMovement.instance.draggingBlock = false;
        if (CameraMovement.instance.selectedObject == gameObject)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        if (TabManager.instance.currentTab == Tab.EDIT && CameraMovement.instance.selectedObject == null)
        {
            CameraMovement.instance.selectedObject = gameObject;
            GetComponent<SpriteRenderer>().sortingOrder = 2;
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            BuildManager.instance.OpenCloseEditOptions(true);
        }
    }

    private void Start()
    {
        switch (block)
        {
            case Entry entry:
                StartCoroutine(EntryCoroutine(entry));
                break;
            default:
                break;
        }
    }

    private IEnumerator EntryCoroutine(Entry entry)
    {
        while (true)
        {
            if (!CameraMovement.instance.gamePaused && !CameraMovement.instance.inMenu && gameObject != CameraMovement.instance.selectedObject)
            {
                entry.SpawnItem();
                yield return new WaitForSeconds(2f);
            }
            else
                yield return null;
        }
    }
}
