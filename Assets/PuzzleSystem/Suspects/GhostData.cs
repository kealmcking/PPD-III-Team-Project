using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostData : MonoBehaviour, ICustomizableComponent
{
    [SerializeField] Ghost ghostPrefab;
    [SerializeField] Sprite icon;
  
    public Ghost Prefab => ghostPrefab;
    public Sprite Icon => icon;
}
