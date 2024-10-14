using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ghost", menuName = "PuzzleSystem/SuspectData/Ghost")]
public class GhostData : ScriptableObject, ICustomizableComponent
{
    [SerializeField] Ghost ghostPrefab;
    [SerializeField] Sprite icon;
  
    public Ghost Prefab => ghostPrefab;
    public Sprite Icon => icon;
}
