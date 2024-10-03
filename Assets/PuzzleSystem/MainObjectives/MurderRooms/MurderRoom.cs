using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
