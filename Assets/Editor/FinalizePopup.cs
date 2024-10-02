using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FinalizePopup : EditorWindow
{
    MonoBehaviour inspectorTarget;
    Case selectedCase;
    List<Case> cases; 
    public static void ShowPopup(MonoBehaviour target)
    {
        var window = GetWindow<FinalizePopup>("Finalize");
        window.inspectorTarget = target;
        window.LoadMotives();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 200);
    }
    private void LoadMotives()
    {
        cases = new List<Case>(Resources.FindObjectsOfTypeAll<Case>());
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a motive for the puzzle", EditorStyles.boldLabel);

        if (cases != null && cases.Count > 0)
        {
            string[] motiveNames = new string[cases.Count];
            for (int i = 0; i < cases.Count; i++)
            {
                motiveNames[i] = cases[i].CaseName;
            }

            int selectedMotiveIndex = cases.IndexOf(selectedCase);
            selectedMotiveIndex = EditorGUILayout.Popup("Select Motive", selectedMotiveIndex, motiveNames);

            if (selectedMotiveIndex >= 0 && selectedMotiveIndex < cases.Count)
            {
                selectedCase = cases[selectedMotiveIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No Motive found. A new motive will be created upon Apply.", MessageType.Info);
        }

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply"))
        {
            FinalizeObject();
            Close();
        }
        if(GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void FinalizeObject()
    {
        if (inspectorTarget == null) return;
        string prefabFolderPath;
        string prefabPath;
        GameObject prefab;
        if(selectedCase == null)
        {
            selectedCase = CreateNewCase();
        }
        if (inspectorTarget is Puzzle puzzle)
        {
            prefabFolderPath = "Assets/PuzzleSystem/PrefabDump/Puzzles";
            prefabPath = $"{prefabFolderPath}/{puzzle.name}.prefab";
            prefab = PrefabUtility.SaveAsPrefabAsset(puzzle.gameObject, prefabPath);
            Puzzle savedPrefab = prefab.GetComponent<Puzzle>();
            selectedCase.Puzzles.Add(savedPrefab);
        }
        if (inspectorTarget is Lore lore)
        {
            prefabFolderPath = "Assets/PuzzleSystem/PrefabDump/Lore";
            prefabPath = $"{prefabFolderPath}/{lore.name}.prefab";
            prefab = PrefabUtility.SaveAsPrefabAsset(lore.gameObject, prefabPath);
            Lore savedPrefab = prefab.GetComponent<Lore>();
            selectedCase.Lore.Add(savedPrefab);
        }
        EditorUtility.SetDirty(selectedCase);
        AssetDatabase.SaveAssets();
    }

    private Case CreateNewCase()
    {
        string caseFolderPath = "Assets/PuzzleSystem/cases/SO";
        Case newCase = CreateInstance<Case>();
        newCase.name = "New Case";
        AssetDatabase.CreateAsset(newCase, $"{caseFolderPath}/NewCase.asset");
        AssetDatabase.SaveAssets();
        return newCase;
    }
}
