using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Object),true)]
public class GenericComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(target is ICustomizableComponent comp)
        {
            if (GUILayout.Button("Finalize"))
            {
                if (comp is Condition condition)
                    ConstructCondition(condition);
                else
                    FinalizePopup.ShowPopup(comp);
            }
        }
        
    }
    private void ConstructCondition(Condition condition)
    {
        string prefabFolderPath = "Assets/PuzzleSystem/PrefabDump/Conditions";
        string prefabPath = $"{prefabFolderPath}/{condition.name}.prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(condition.gameObject, prefabPath);
    }
}
