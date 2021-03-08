using UnityEngine;

[CreateAssetMenu(fileName = "New ItemUI", menuName = "ItemUI")]
public class ItemUI : ScriptableObject
{
    public Sprite sprite;
    [HideInInspector] public bool isInput;
    [HideInInspector] public float price;
}
