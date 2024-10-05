using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents the room of the killer. used for UI and for checking against the chosen murder room 
/// to determine if the players guess for the room is correct.
/// </summary>
[CreateAssetMenu(fileName = "Room", menuName = "PuzzleSystem/Killer/MurderRoom")]
public class MurderRoom : ScriptableObject, ICustomizableComponent
{
    [SerializeField] string roomName;
    [SerializeField] Sprite icon;
    [SerializeField] Description description;

    public string Name => roomName;
    public Sprite Icon => icon;
    public Description Description => description;

}
