using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Lore))]
public class LoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Lore lore = (Lore)target;
        if (GUILayout.Button("Finalize"))
        {
            FinalizePopup.ShowPopup(lore);
        }
    }
}
