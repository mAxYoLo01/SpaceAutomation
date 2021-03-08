using System;
using UnityEngine;

[Serializable]
public class Block : IEquatable<Block>
{
    public int id;
    public string blockId;
    public Position position;
    public int orientation;
    [NonSerialized]
    public BlockUI blockUI;

    public Block(Vector2Int position)
    {
        this.position = position;
        this.orientation = 0;
        this.id = Utils.GenerateRandomId();
        GenerateBlockUI();
    }

    public void GenerateBlockUI()
    {
        blockUI = Resources.Load<BlockUI>("UIs/Blocks/" + blockId);
    }

    public bool Equals(Block other)
    {
        return this.id == other.id;
    }

    public virtual GameObject OpenBlockGUI(Block block)
    {
        string gui;
        switch (block)
        {
            case Entry entry:
                gui = "Entry";
                break;
            case Machine machine:
                gui = "Machine";
                break;
            default:
                return null;
        }
        Transform t = BuildManager.instance.blockGUIs.Find(gui);
        if (t != null)
        {
            CameraMovement.instance.inMenu = true;
            t.gameObject.SetActive(true);
            return t.gameObject;
        }
        return null;
    }
}

[Serializable]
public struct Position
{
    public int x;
    public int y;

    public Position(Vector2Int vec)
    {
        x = vec.x;
        y = vec.y;
    }

    public static implicit operator Vector2Int(Position pos) => new Vector2Int(pos.x, pos.y);
    public static implicit operator Vector2(Position pos) => new Vector2(pos.x, pos.y);
    public static implicit operator Position(Vector2Int vec) => new Position(vec);

    public static Position operator +(Position left, Position right)
    {
        return new Position() { x = left.x + right.x, y = left.y + right.y };
    }
}