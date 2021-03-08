using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public Tilemap tileMap;
    public TileBase tile;

    public static MapGenerator instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MapGenerator found!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        tileMap.ClearAllTiles();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tileMap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, Camera.main.transform.position.z);
    }

    public GameObject CreateBlockUI(Block block)
    {
        block.GenerateBlockUI();
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Block"));
        go.transform.localPosition = new Vector3(block.position.x + 0.5f, block.position.y + 0.5f, 0);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(block.blockUI.width, block.blockUI.height);
        go.GetComponent<RectTransform>().pivot = block.blockUI.sprite.pivot;
        go.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, block.orientation * 90f);
        go.GetComponent<BoxCollider2D>().size = new Vector2(block.blockUI.width, block.blockUI.height);
        go.GetComponent<BoxCollider2D>().offset = (new Vector2(block.blockUI.width, block.blockUI.height) - Vector2.one) / 2;
        go.GetComponent<SpriteRenderer>().sprite = block.blockUI.sprite;
        go.GetComponent<BlockBehaviour>().block = block;
        go.name = $"{block.blockId} {block.id}";
        return go;
    }
}
