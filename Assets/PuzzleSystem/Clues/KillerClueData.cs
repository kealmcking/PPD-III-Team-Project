
using UnityEngine;
/// <summary>
/// Represents a clue that hints towards who the killer is not
/// </summary>

public class KillerClueData : BaseClueData
{
    [SerializeField] SuspectData suspectData;
    public SuspectData Suspect => suspectData;
    public void SetName(string name)
    {
        itemName = name; 
    }
    public void SetIcon(Sprite icon)
    {
        this.icon = icon;
    }
    public void SetSuspectData(SuspectData value)
    {
        suspectData = value;
    }
    public void SetDescription(Description description)
    {
        this.description = description;
    }

}
