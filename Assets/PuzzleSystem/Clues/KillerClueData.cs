
using UnityEngine;
/// <summary>
/// Represents a clue that hints towards who the killer is not
/// </summary>

public class KillerClueData : BaseClueData
{
    [SerializeField] Suspect prefab;
    public new Suspect Prefab => prefab;
    public void SetName(string name)
    {
        itemName = name; 
    }
    public void SetIcon(Sprite icon)
    {
        this.icon = icon;
    }
    public void SetPrefab(Suspect prefab)
    {
        this.prefab = prefab;
    }
    public void SetDescription(Description description)
    {
        this.description = description;
    }

}
