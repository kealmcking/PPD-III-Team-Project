using System;
using UnityEngine;
[CreateAssetMenu(fileName = "SuspectData", menuName = "PuzzleSystem/SuspectData/Suspect")]
public class SuspectData : ScriptableObject, ICustomizableComponent
{
    [SerializeField] string suspectName;
    [SerializeField] Suspect suspectPrefab;
    [SerializeField] Sprite icon;
    [SerializeField] NPC npc;
    [SerializeField] Description description;
    Guid id = new Guid();
    public Guid ID => id;
    public string Name => suspectName;
    public Suspect SuspectPrefab => suspectPrefab;
    public Sprite Icon => icon;
    public NPC Npc { get { return npc; } set { npc = value; } }
    public Description Description => description;  
}
