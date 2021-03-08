using UnityEngine;

public class Saw : Machine
{
    public Saw(Vector2Int position) : base(position)
    {
        this.blockId = "saw";
        GenerateMachineUI();
    }
}