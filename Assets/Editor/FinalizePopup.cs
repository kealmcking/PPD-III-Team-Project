using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using static UnityEngine.AudioSettings;


public class FinalizePopup : EditorWindow
{
    ICustomizableComponent inspectorTarget;
    Case selectedCase;
    Condition selectedCondition;
    ScriptableObject selectedData;
    BaseItemData selectedItemData;
    Director selectedDirector;
    UnityEngine.Object selectedObj;
    Item selectedItem;

    List<Case> cases;
    List<Item> items;
    List<Puzzle> puzzles;
    List<Condition> conditions;
    List<BaseItemData> itemDatas;
    List<Item> itemObjs;
    List<Director> directors;
    List<UnityEngine.Object> mergedList;
    Dictionary<ScriptableObject,FieldInfo> fields;

    string prefabFolderPath;
    string prefabPath;
    GameObject prefab;
    public static void ShowPopup(ICustomizableComponent target)
    {
        var window = GetWindow<FinalizePopup>("Finalize");
        window.inspectorTarget = target;
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 200);
    }
    
    private void OnGUI()
    {
        if (inspectorTarget == null) return;     
        switch (inspectorTarget)
        {
            case Puzzle puzzle:
                LoadCases();
                DisplayCases();
                DisplayPuzzleButtons(puzzle);                
                break;
            case Lore lore:
                LoadCases();
                DisplayCases();
                DisplayLoreButtons(lore);                
                break;
            case ConditionConfig config:
                LoadConditions();
                DisplayConditions();
                DisplayConditionConfigButtons(config);
                break;
            case Description description:
                LoadDescriptionObjects();
                DisplayDescriptions();
                DisplayDescriptionButtons(description);
                break;
            case MurderRoom room:
                LoadDirectors();
                DisplayDirectors();
                DisplayMurderRoomButtons(room);
                break;
            case MurderWeapon weapon:
                LoadDirectors();
                DisplayDirectors();
                DisplayMurderWeaponButtons(weapon);
                break;
            case MurderMotive motive:
                LoadDirectors();
                DisplayDirectors();
                DisplayMurderMotiveButtons(motive);
                break;
            case Case caseObj:
                LoadDirectors();
                DisplayDirectors();
                DisplayCaseButtons(caseObj);
                break;
            case Item item:
                Type clue = typeof(BaseClueData);
                Type craftItem = typeof(CraftableItemData);
                Type craftComponent = typeof(CraftableComponentData);
                Type[] filter = { clue, craftItem, craftComponent };
                LoadBaseItemData(filter);
                DisplayBaseItemData();
                DisplayItemButtons(item);
                break;
            case CraftableComponentData data:
                LoadConditions();
                Type craftItemData = typeof(CraftableItemData);
                Type[] compFilter = { craftItemData};
                LoadBaseItemData(compFilter);
                MergeListsAndDisplay();
                DisplayCraftableComponentDataButtons(data);
                break;
            case CraftableItemData data:
                LoadConditions();
                DisplayConditions();
                DisplayCraftableItemDataButtons(data);
                break;
            case BaseItemData itemData:
                LoadItem();
                DisplayItem();
                DisplayItemDataButtons(itemData);
                break;
           
            default:
                break;
        }       
        AssetDatabase.SaveAssets();
    }

  

    private void LoadItem()
    {
        itemObjs = new List<Item>(Resources.FindObjectsOfTypeAll<Item>());
    }
    private void LoadDirectors()
    {
        directors = new List<Director>(Resources.FindObjectsOfTypeAll<Director>());
    }
    private void LoadCases()
    {
        cases = new List<Case>(Resources.FindObjectsOfTypeAll<Case>());
    }
    private void LoadDescriptionObjects()
    {
        var all = Resources.FindObjectsOfTypeAll<ScriptableObject>();
        foreach (var item in all)
        {
            Type itemType = item.GetType();
            FieldInfo[] field = itemType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var info in field)
            {
                if (info.FieldType == typeof(Description))
                {
                    fields.Add(item, info);
                }
            }
        }
    }
    private void LoadConditions()
    {
        conditions = new List<Condition>(Resources.FindObjectsOfTypeAll<Condition>());
    }
    private void LoadBaseItemData(Type[] filter = null)
    {
        if(filter != null && filter.Length > 0) {
            foreach (var type in filter)
            {
                itemDatas.AddRange(Resources.FindObjectsOfTypeAll(type));
            }
        }
    }



    private void DisplayItem()
    {
        GUILayout.Label("Select a item for the data", EditorStyles.boldLabel);

        if (itemObjs != null && itemObjs.Count > 0)
        {
            string[] names = new string[itemObjs.Count];
            for (int i = 0; i < itemObjs.Count; i++)
            {
                names[i] = itemObjs[i].name;
            }
            Array.Sort(names);
            int selectedIndex = itemObjs.IndexOf(selectedItem);
            selectedIndex = EditorGUILayout.Popup("Select Item", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < itemObjs.Count)
            {
                selectedItem = itemObjs[selectedIndex];
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No Items found. Create an item first.", MessageType.Info);
        }
    }
    private void DisplayDirectors()
    {
        GUILayout.Label("Select a Director for the data", EditorStyles.boldLabel);

        if (directors != null && directors.Count > 0)
        {
            string[] names = new string[directors.Count];
            for (int i = 0; i < directors.Count; i++)
            {
                names[i] = directors[i].name;
            }
            Array.Sort(names);
            int selectedIndex = directors.IndexOf(selectedDirector);
            selectedIndex = EditorGUILayout.Popup("Select Director", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < directors.Count)
            {
                selectedDirector = directors[selectedIndex];
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No Directors found. Create a Director first.", MessageType.Info);
        }
    }
    private void DisplayConditions()
    {
        GUILayout.Label("Select a condition for the config", EditorStyles.boldLabel);

        if (conditions != null && conditions.Count > 0)
        {
            string[] names = new string[conditions.Count];
            for (int i = 0; i < conditions.Count; i++)
            {
                names[i] = conditions[i].name;
            }
            Array.Sort(names);
            int selectedIndex = conditions.IndexOf(selectedCondition);
            selectedIndex = EditorGUILayout.Popup("Select Condition", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < conditions.Count)
            {
                selectedCondition = conditions[selectedIndex];
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No conditions found. Create a condition first.", MessageType.Info);
        }
    }
    private void DisplayCases()
    {
        GUILayout.Label("Select a case for the puzzle or lore", EditorStyles.boldLabel);

        if ( cases != null && cases.Count > 0)
        {
            string[] names = new string[cases.Count];
            for (int i = 0; i < cases.Count; i++)
            {
                names[i] = cases[i].name;
            }
            Array.Sort(names);
            int selectedIndex = cases.IndexOf(selectedCase);
            selectedIndex = EditorGUILayout.Popup("Select Case", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < cases.Count)
            {
                selectedCase = cases[selectedIndex];
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No cases found. Create a case first.", MessageType.Info);
        }
    }
    private void DisplayDescriptions()
    {
        GUILayout.Label("Select a file for the description", EditorStyles.boldLabel);

        if(fields != null && fields.Count > 0)
        {
            string[] options = new string[fields.Count];
            for(int i = 0;i < fields.Count; i++)
            {
                options[i] = fields.Keys.ElementAt(i).name;
            }
            Array.Sort(options);
            int selectedIndex = fields.Keys.ToList().IndexOf(selectedData);
            selectedIndex = EditorGUILayout.Popup(selectedIndex, options);

            selectedData = fields.Keys.ElementAt(selectedIndex);
        }
    }
    private void DisplayBaseItemData()
    {
        GUILayout.Label("Select an data file for the item", EditorStyles.boldLabel);

        if (itemDatas != null && itemDatas.Count > 0)
        {
            string[] names = new string[itemDatas.Count];
            for (int i = 0; i < itemDatas.Count; i++)
            {
                names[i] = itemDatas[i].name;
            }
            Array.Sort(names);
            int selectedIndex = itemDatas.IndexOf(selectedItemData);
            selectedIndex = EditorGUILayout.Popup("Select Data", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < itemDatas.Count)
            {
                selectedItemData = itemDatas[selectedIndex];
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No Data found. Create a scriptable object that can hold an item first.", MessageType.Info);
        }
    }
    private void MergeListsAndDisplay()
    {
        mergedList.AddRange(conditions);
        mergedList.AddRange(itemDatas);
        GUILayout.Label("Select an object for the item", EditorStyles.boldLabel);

        if (mergedList != null && mergedList.Count > 0)
        {
            string[] names = new string[mergedList.Count];
            for (int i = 0; i < mergedList.Count; i++)
            {
                names[i] = mergedList[i].name;
            }
            Array.Sort(names);
            int selectedIndex = mergedList.IndexOf(selectedObj);
            selectedIndex = EditorGUILayout.Popup("Select Object", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < mergedList.Count)
            {
                selectedObj = mergedList[selectedIndex];
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No Object found. Create a Object that can hold an item first.", MessageType.Info);
        }
    }


    private void DisplayItemButtons(Item item)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && mergedList.Count > 0)
        {
            ConstructItem(item);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayConditionConfigButtons(ConditionConfig config)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && conditions.Count > 0)
        {
            ConstructConditionConfig(config);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayPuzzleButtons(Puzzle puzzle)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && cases.Count > 0)
        {
            ConstructPuzzle(puzzle);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayLoreButtons(Lore lore)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && cases.Count > 0)
        {
            ConstructLore(lore);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayDescriptionButtons(Description description)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && fields.Count > 0)
        {
            ConstructDescription(description);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayCaseButtons(Case caseObj)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && directors.Count > 0)
        {
            ConstructCase(caseObj);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayMurderMotiveButtons(MurderMotive motive)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && directors.Count > 0)
        {
            ConstructMurderMotive(motive);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayMurderWeaponButtons(MurderWeapon weapon)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && directors.Count > 0)
        {
            ConstructMurderWeapon(weapon);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayMurderRoomButtons(MurderRoom room)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && directors.Count > 0)
        {
            ConstructMurderRoom(room);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayItemDataButtons(BaseItemData itemData)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && itemObjs.Count > 0)
        {
            ConstructItemData(itemData);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayCraftableComponentDataButtons(CraftableComponentData data)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && itemObjs.Count > 0)
        {
            ConstructCraftableComponentData(data);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayCraftableItemDataButtons(CraftableItemData data)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && itemObjs.Count > 0)
        {
            ConstructCraftableItemData(data);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }

  

    private void ConstructPuzzle(Puzzle puzzle)
    {
        prefabFolderPath = "Assets/PuzzleSystem/PrefabDump/Puzzles";
        prefabPath = $"{prefabFolderPath}/{puzzle.name}.prefab";
        prefab = PrefabUtility.SaveAsPrefabAsset(puzzle.gameObject, prefabPath);
        Puzzle savedPrefab = prefab.GetComponent<Puzzle>();
        selectedCase.Puzzles.Add(savedPrefab);
        EditorUtility.SetDirty(selectedCase);
    }
    private void ConstructLore(Lore lore)
    {
        prefabFolderPath = "Assets/PuzzleSystem/PrefabDump/Lore";
        prefabPath = $"{prefabFolderPath}/{lore.name}.prefab";
        prefab = PrefabUtility.SaveAsPrefabAsset(lore.gameObject, prefabPath);
        Lore savedPrefab = prefab.GetComponent<Lore>();
        selectedCase.Lore.Add(savedPrefab);
        EditorUtility.SetDirty(selectedCase);
    }
    private void ConstructConditionConfig(ConditionConfig config)
    {
        FieldInfo[] fields = selectedCondition.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo info = fields.FirstOrDefault(field => field.FieldType == typeof(ConditionConfig));
        info.SetValue(selectedCondition, config);
        EditorUtility.SetDirty(selectedCondition);
    }
    private void ConstructDescription(Description discription)
    {
        fields.TryGetValue(selectedData, out FieldInfo value);
        if(value != null) 
         value.SetValue(selectedData, discription);    
    }
    private void ConstructCase(Case caseObj)
    {
        selectedDirector.Cases.Append(caseObj);
        EditorUtility.SetDirty(selectedDirector);
    }
    private void ConstructMurderRoom(MurderRoom room)
    {
        selectedDirector.Rooms.Append(room);
        EditorUtility.SetDirty(selectedDirector);
    }
    private void ConstructMurderWeapon(MurderWeapon weapon)
    {
        selectedDirector.Weapons.Append(weapon);
        EditorUtility.SetDirty(selectedDirector);
    }
    private void ConstructMurderMotive(MurderMotive motive)
    {
        selectedDirector.Motives.Append(motive);
        EditorUtility.SetDirty(selectedDirector);
    }
    private void ConstructItem(Item item)
    {
        prefabFolderPath = "Assets/PuzzleSystem/PrefabDump/Item";
        prefabPath = $"{prefabFolderPath}/{item.name}.prefab";
        prefab = PrefabUtility.SaveAsPrefabAsset(item.gameObject, prefabPath);
        Item savedPrefab = prefab.GetComponent<Item>();
        FieldInfo[] fields = selectedItemData.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo info = fields.FirstOrDefault(field => field.FieldType == typeof(Item));
        info.SetValue(selectedItemData, savedPrefab);
        EditorUtility.SetDirty(selectedItemData);
    }
    private void ConstructItemData(BaseItemData itemData)
    {
        FieldInfo[] fields = selectedItem.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo info = fields.FirstOrDefault(field => field.FieldType == typeof(BaseItemData));
        info.SetValue(selectedItem, itemData);
        EditorUtility.SetDirty(selectedItem);
    }
    private void ConstructCraftableComponentData(CraftableComponentData data)
    {
        FieldInfo[] fields = selectedItem.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo info = fields.FirstOrDefault(field => field.FieldType == typeof(CraftableComponentData));
        info.SetValue(selectedItem, data);
        EditorUtility.SetDirty(selectedItem);
    }
    private void ConstructCraftableItemData(CraftableItemData data)
    {
        FieldInfo[] fields = selectedItem.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo info = fields.FirstOrDefault(field => field.FieldType == typeof(CraftableItemData));
        info.SetValue(selectedItem, data);
        EditorUtility.SetDirty(selectedItem);
    }
}
