using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Description", menuName = "PuzzleSystem/Utils/Description")]
public class Description : ScriptableObject, ICustomizableComponent
{
    [SerializeField, TextArea] string description;
    public string Text => description;
}
