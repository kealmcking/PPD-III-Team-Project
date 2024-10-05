using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Allows a simple way to track the suspects who are in the game and whether or not they are the killer. 
/// </summary>
public class Suspect : MonoBehaviour
{
    [SerializeField] string suspectName;
    [SerializeField] Sprite icon;
    public bool IsKiller { get; set; } = false;
    public string Name => suspectName;  
    public Sprite Icon => icon;
}
