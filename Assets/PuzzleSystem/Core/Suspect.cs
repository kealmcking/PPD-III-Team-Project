using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    [SerializeField] string suspectName;
    public bool IsKiller { get; set; } = false;
    public string Name => suspectName;  
}
