using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Recipe", menuName = "Scriptable Objects/Recipes")]
public class itemRecipeScriptableObject : ScriptableObject
{
    public string recipeName;

    public itemRecipeScriptableObject[] input;
    public itemRecipeScriptableObject[] result;

}