using UnityEngine;
[CreateAssetMenu(fileName = "SuspectData", menuName = "PuzzleSystem/SuspectData/Suspect")]
public class SuspectData : ScriptableObject, ICustomizableComponent
{
    [SerializeField] string suspectName;
    [SerializeField] Suspect suspectPrefab;
    [SerializeField] Sprite icon;
    [SerializeField] NPC npc;
    [SerializeField] Description description;
    public string Name => suspectName;
    public Suspect SuspectPrefab => suspectPrefab;
    public Sprite Icon => icon;
    public NPC Npc => npc;
    public Description Description => description;  
}
