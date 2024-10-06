using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A helpful class for generating a basic description for any item. 
/// </summary>
[CreateAssetMenu(fileName = "Description", menuName = "PuzzleSystem/Utils/Description")]
public class Description : ScriptableObject, ICustomizableComponent
{
    [SerializeField, TextArea] string description;
    public string Text => description;
}
