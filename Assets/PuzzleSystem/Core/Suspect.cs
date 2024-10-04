using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    [SerializeField] string suspectName;
    [SerializeField] Sprite icon;
    public bool IsKiller { get; set; } = false;
    public string Name => suspectName;  
    public Sprite Icon => icon;
}
