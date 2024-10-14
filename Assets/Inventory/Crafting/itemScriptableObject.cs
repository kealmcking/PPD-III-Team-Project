using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class itemScriptableObject : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    //public int maxStack; if there are multiple of one crafting component in the future
}