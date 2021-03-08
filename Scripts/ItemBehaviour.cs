using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public Item item;

    float speed = 1f;
    Block currentBlock;
    Vector3 currentTarget;
    bool firstPartOfPath = true;

    public void Setup(Block block)
    {
        currentBlock = block;
        transform.position = Utils.OrientatedPosition(currentBlock, currentBlock.position + 0.5f * Vector2.one, currentBlock.blockUI.outputs[0]);
        SetCurrentBlock();
        SetCurrentTarget();
    }

    void Update()
    {
        if (!CameraMovement.instance.gamePaused && !CameraMovement.instance.inMenu)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentTarget) < 0.001f)
            {
                if (firstPartOfPath)
                {
                    firstPartOfPath = false;
                }
                else
                {
                    firstPartOfPath = true;
                    SetCurrentBlock();
                }
                SetCurrentTarget();
            }
        }
    }

    void SetCurrentBlock()
    {
        currentBlock = Utils.GetBlockWithInputOutputCorresponding(Utils.OrientatedPosition(currentBlock, currentBlock.position + 0.5f * Vector2.one, currentBlock.blockUI.outputs[0]));
    }

    void SetCurrentTarget()
    {
        switch (currentBlock)
        {
            case Conveyor conveyor:
                if (firstPartOfPath)
                    currentTarget = Utils.OrientatedPosition(currentBlock, currentBlock.position + 0.5f * Vector2.one, Vector2.zero);
                else
                    currentTarget = Utils.OrientatedPosition(currentBlock, currentBlock.position + 0.5f * Vector2.one, currentBlock.blockUI.outputs[0]);
                break;
            case Entry entry:
                currentTarget = Utils.OrientatedPosition(currentBlock, currentBlock.position + 0.5f * Vector2.one, currentBlock.blockUI.outputs[0]);
                firstPartOfPath = false;
                break;
            case Exit exit:
                exit.SellItem(item);
                Destroy(gameObject);
                break;
            case Machine machine:
                machine.AddItemInInventory(item);
                BuildManager.instance.StartCoroutine(machine.TryCraft());
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
