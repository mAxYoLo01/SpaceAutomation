using UnityEngine;

public class Furnace : Machine
{
    public Furnace(Vector2Int position) : base(position)
    {
        this.blockId = "furnace";
        GenerateMachineUI();
    }
}