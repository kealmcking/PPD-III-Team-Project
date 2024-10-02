using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Puzzle))]
public class PuzzleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Puzzle puzzle = (Puzzle)target;
        if (GUILayout.Button("Finalize"))
        {
            FinalizePopup.ShowPopup(puzzle);
        }
    }
}
