using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Description", menuName = "PuzzleSystem/Utils/Description")]
public class Description : ScriptableObject
{
    [SerializeField, TextArea] string description;
    public string Text => description;
}
