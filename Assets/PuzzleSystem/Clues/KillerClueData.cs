
using UnityEngine;
/// <summary>
/// Represents a clue that hints towards who the killer is not
/// </summary>
[CreateAssetMenu(fileName = "KillerClue", menuName = "PuzzleSystem/Clues/KillerClue")]
public class KillerClueData : BaseClueData
{  
    public void SetName(string name)
    {
        itemName = name; 
    }
    public void SetIcon(Sprite icon)
    {
        this.icon = icon;
    }
   
    public void SetDescription(Description description)
    {
        this.description = description;
    }

}
