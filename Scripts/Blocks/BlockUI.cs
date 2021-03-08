using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BlockUI", menuName = "BlockUI")]
public class BlockUI : ScriptableObject
{
    [HideInInspector] public int width;
    [HideInInspector] public int height;
    public float cost;
    public Sprite sprite;
    public List<Vector2> inputs;
    public List<Vector2> outputs;
}
