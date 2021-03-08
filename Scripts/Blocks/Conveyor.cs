using UnityEngine;

public class Conveyor : Block
{

    public Conveyor(Vector2Int position) : base(position)
    {
        this.blockId = "conveyor";
    }
}
