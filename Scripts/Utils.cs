using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Block GetBlockFromId(string blockId, Vector2Int position)
    {
        switch (blockId)
        {
            case "entry":
                return new Entry(position);
            case "conveyor":
                return new Conveyor(position);
            case "exit":
                return new Exit(position);
            case "saw":
                return new Saw(position);
            case "furnace":
                return new Furnace(position);
            default:
                return null;
        }
    }

    public static int GenerateRandomId()
    {
        return Random.Range(0, 99999999);
    }

    public static bool IsPositionValid(Vector2 futurePos, Block block)
    {
        List<Vector2Int> takenPositions = GetAllTakenPositions(block);
        /*foreach (Vector2Int pos in takenPositions)
        {
            Debug.Log($"X: {pos.x} Y: {pos.y}");
        }*/

        for (int i = 0; i < block.blockUI.width; i++)
        {
            for (int j = 0; j < block.blockUI.height; j++)
            {
                Vector2Int position = Vector2Int.RoundToInt(OrientatedPosition(block, futurePos, new Vector2(i, j)));
                if (position.x < 0 || position.x >= MapGenerator.instance.width)
                {
                    return false;
                }
                if (position.y < 0 || position.y >= MapGenerator.instance.height)
                {
                    return false;
                }
                if (takenPositions.Contains(position))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static List<Vector2Int> GetAllTakenPositions(Block excludedBlock)
    {
        List<Vector2Int> takenPositions = new List<Vector2Int>();
        foreach (Transform blockT in GameObject.Find("Blocks").transform)
        {
            Block block = blockT.GetComponent<BlockBehaviour>().block;
            if (excludedBlock != block)
            {
                for (int i = 0; i < block.blockUI.width; i++)
                {
                    for (int j = 0; j < block.blockUI.height; j++)
                    {
                        takenPositions.Add(Vector2Int.RoundToInt(OrientatedPosition(block, block.position, new Vector2(i, j))));
                    }
                }
            }
        }
        return takenPositions;
    }

    public static Vector2 OrientatedPosition(Block block, Vector2 position, Vector2 offset)
    {
        Vector2 finalOffset;
        switch (block.orientation)
        {
            case 0:
            default:
                finalOffset = offset;
                break;
            case 1:
                finalOffset = new Vector2(-offset.y, offset.x);
                break;
            case 2:
                finalOffset = -offset;
                break;
            case 3:
                finalOffset = new Vector2(offset.y, -offset.x);
                break;
        }
        return position + finalOffset;
    }

    public static Block GetBlockWithInputOutputCorresponding(Vector2 output)
    {
        foreach (Transform blockT in GameObject.Find("Blocks").transform)
        {
            Block block = blockT.GetComponent<BlockBehaviour>().block;
            foreach (Vector2 offset in block.blockUI.inputs)
            {
                if (Vector3.Distance(output, Utils.OrientatedPosition(block, block.position + 0.5f * Vector2.one, offset)) < 0.001f)
                {
                    return block;
                }
            }
        }
        return null;
    }
}
