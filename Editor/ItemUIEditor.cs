using UnityEditor;

[CustomEditor(typeof(ItemUI))]
public class ItemUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ItemUI itemUI = (ItemUI)target;
        itemUI.isInput = EditorGUILayout.Toggle("Is Input?", itemUI.isInput);
        if (itemUI.isInput)
        {
            itemUI.price = EditorGUILayout.FloatField("Cost", itemUI.price);
        }
        else
        {
            itemUI.price = EditorGUILayout.FloatField("Price", itemUI.price);
        }
    }
}
