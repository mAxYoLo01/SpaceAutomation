using UnityEditor;

[CustomEditor(typeof(BlockUI))]
public class BlockUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BlockUI blockUI = (BlockUI)target;
        EditorGUILayout.LabelField("Size of the block:");
        EditorGUILayout.BeginHorizontal();
        blockUI.width = EditorGUILayout.IntField("Width", blockUI.width);
        blockUI.height = EditorGUILayout.IntField("Height", blockUI.height);
        EditorGUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}
