using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions.EasingCore;


/// <summary>
/// Loads all classes/objects pertaining to the puzzle system. 
/// used for easily assigning a newly made puzzle system object to a field of its same type
/// </summary>
public class FinalizePopup : EditorWindow
{
    static ICustomizableComponent inspectorTarget;
    static KeyValuePair<UnityEngine.Object, FieldInfo> selectedKey = new KeyValuePair<UnityEngine.Object, FieldInfo>();
    static List<UnityEngine.Object> classScripts = new List<UnityEngine.Object>();
    static Dictionary<UnityEngine.Object,FieldInfo> filteredScripts = new Dictionary<UnityEngine.Object, FieldInfo>();
   // public bool isLoaded = false;
    string prefabFolderPath;
    string prefabPath;
    GameObject prefab;
    /// <summary>
    /// Initializes the classes used in the puzzle system. This also resets statics to their default states. 
    /// </summary>
    [InitializeOnLoadMethod]
    static void InitializeList()
    {
        inspectorTarget = null;
        selectedKey = new KeyValuePair<UnityEngine.Object, FieldInfo>();
        classScripts = new List<UnityEngine.Object>();
        filteredScripts = new Dictionary<UnityEngine.Object, FieldInfo>();


        LoadAllSOOfType<Case>();
        LoadAllSOOfType<BaseItemData>();
        LoadAllSOOfType<ConditionConfig>();
        LoadAllSOOfType<MurderMotive>();
        LoadAllSOOfType<MurderRoom>();
        LoadAllSOOfType<MurderWeapon>();
        LoadAllSOOfType<BaseClueData>();
        LoadAllMonoOfType<Lore>();
        LoadAllMonoOfType<Item>();
        LoadAllMonoOfType<Director>();
        LoadAllMonoOfType<Puzzle>();
        LoadAllMonoOfType<Condition>();
       
    }
    /// <summary>
    /// Used statically from the generic component button script to set specific parameters pertaining to the selected script
    /// </summary>
    /// <param name="target"></param>
    public static void ShowPopup(ICustomizableComponent target)
    {     
        var window = GetWindow<FinalizePopup>("Finalize");
        
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 200);
        InitializeList();
        inspectorTarget = target;
      
    }
    /// <summary>
    /// Contains two buttons. Cancel button closes the window. Apply button will determine if the object is derived from monobehaviour 
    /// it then creates a prefab and adds it to the prefab dump directory. After this the selected script from the filtered dropdown is prepared
    /// to have the script that opened this window to be added to the appropriate field on the selected script. 
    /// The field can either be of type IList or a standard variable field.  
    /// </summary>
    private void OnGUI()
    {
        if (inspectorTarget == null) return;
        Filter();
        Display();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply") && filteredScripts.Count > 0)
        {

            Type type = selectedKey.Value.FieldType;

            if (inspectorTarget is MonoBehaviour mono)
            {
                prefabFolderPath = $"Assets/PuzzleSystem/PrefabDump";
                prefabPath = $"{prefabFolderPath}/{mono.name}.prefab";
                prefab = PrefabUtility.SaveAsPrefabAsset(mono.gameObject, prefabPath);


                GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, mono.gameObject.scene);

                if (typeof(IList).IsAssignableFrom(type))
                {
                    IList list = (IList)selectedKey.Value.GetValue(selectedKey.Key);
                    if (list != null)
                    {
                        list.Add(prefab.gameObject.GetComponent(type.GetGenericArguments()[0]));
                       
                    }
                }
                else
                {
                    selectedKey.Value.SetValue(selectedKey.Key, prefab.gameObject.GetComponent(type));

                }
                DestroyImmediate(mono.gameObject);
                EditorUtility.SetDirty(selectedKey.Key);

            }
               
            if(inspectorTarget is ScriptableObject scriptable)
            {
                if (typeof(IList).IsAssignableFrom(type))
                {
                    IList list = (IList)selectedKey.Value.GetValue(selectedKey.Key);
                    if (list != null)
                    {
                        list.Add(scriptable);
                    }
                }
                else
                {
                    selectedKey.Value.SetValue(selectedKey.Key, scriptable);          
                }

                EditorUtility.SetDirty(selectedKey.Key);

                if(selectedKey.Key is GameObject prefab)
                {
                    GameObject[] all = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    foreach(var obj in all)
                    {
                        if(PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular && PrefabUtility.GetCorrespondingObjectFromSource(obj)==prefab)
                        {
                            if (typeof(IList).IsAssignableFrom(type))
                            {
                                IList list = (IList)selectedKey.Value.GetValue(obj);
                                if (list != null)
                                {
                                    list.Add(scriptable);
                                }
                            }
                            else
                            {
                                selectedKey.Value.SetValue(obj, scriptable);
                            }
                            PrefabUtility.ApplyPrefabInstance(obj, InteractionMode.UserAction);
                            EditorUtility.SetDirty(obj);
                        }
                    }
                }
            }
            AssetDatabase.SaveAssets();
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
        
    }
    /// <summary>
    /// Used to appropriatly load any class/object if it is derived from monobehaviour.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static private void LoadAllMonoOfType<T>() where T : MonoBehaviour
    {
        List<T> foundObjects = new List<T>();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                T component = prefab.GetComponent<T>();
                if (component != null)
                {                    
                    classScripts.Add(component);
                }

                T[] componentsInChildren = prefab.GetComponentsInChildren<T>(true);
                if (componentsInChildren.Length > 0)
                {                   
                    classScripts.AddRange(componentsInChildren);
                }
            }
        }

        T[] sceneObjects = Resources.FindObjectsOfTypeAll<T>();
        foreach (T obj in sceneObjects)
        {
            GameObject objGameObject = obj.gameObject;
            if (objGameObject.scene.name != null)
            {              
                classScripts.Add(obj);
            }
        }      
    }
    /// <summary>
    /// Used to appropriatly load any class/object if it is derived from scriptable object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static private void LoadAllSOOfType<T>() where T : ScriptableObject
    {
        string[] assetGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        if (assetGuids.Length <= 0) return;
        foreach(string guid in assetGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if(asset != null)
            {
                classScripts.Add(asset);              
            }
        }
        T[] resourceAssets = Resources.FindObjectsOfTypeAll<T>();
        foreach (T asset in resourceAssets)
        {
            if (!classScripts.Contains(asset))
            {
                classScripts.Add(asset);               
            }
        }
    }
    /// <summary>
    /// Used to filter the loaded classes by which class contains the specific fields that the selected item can be placed on
    /// </summary>
    static private void Filter()
    {
        if (classScripts == null || classScripts.Count <= 0) return;

     
        Type targetType = inspectorTarget.GetType();
        
        foreach (var item in classScripts)
        {
            
            Type itemType = item.GetType();
            
            FieldInfo[] fields = itemType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
              
                if (field.FieldType == targetType || field.FieldType.IsSubclassOf(targetType)|| field.FieldType == targetType.BaseType)
                {
                    if (!filteredScripts.ContainsKey(item))
                    {
                        filteredScripts.Add(item, field);
                    
                    }
                }
              
                else if (typeof(IList).IsAssignableFrom(field.FieldType) && field.FieldType.IsGenericType)
                {
                    Type listElementType = field.FieldType.GetGenericArguments()[0];
                    if (listElementType == targetType || listElementType.IsSubclassOf(targetType))
                    {
                        if (!filteredScripts.ContainsKey(item))
                        {
                            filteredScripts.Add(item, field);
                           
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Used to display the filtered scripts by name in a dropdown list. 
    /// </summary>
    static private void Display()
    {
        GUILayout.Label("Select a item for the data", EditorStyles.boldLabel);

        if (filteredScripts != null && filteredScripts.Count > 0)
        {
            string[] names = new string[filteredScripts.Count];
            for (int i = 0; i < filteredScripts.Count; i++)
            {
                names[i] = filteredScripts.Keys.ElementAt(i).name;
            }
            List<UnityEngine.Object> keysList = filteredScripts.Keys.ToList();
            int selectedIndex = keysList.IndexOf(selectedKey.Key);
            selectedIndex = EditorGUILayout.Popup("Select option", selectedIndex, names);

            if (selectedIndex >= 0 && selectedIndex < filteredScripts.Count)
            {
                UnityEngine.Object obj = keysList[selectedIndex];
                selectedKey = new KeyValuePair<UnityEngine.Object, FieldInfo>(obj, filteredScripts[obj]);
            }

        }
        else
        {
            EditorGUILayout.HelpBox("No Items found. Create an item first.", MessageType.Info);
        }
    }
}
