using UnityEngine;
/// <summary>
/// Represents the payload that is used during an interactionwith any possible interactable. 
/// </summary>
public struct Payload
{
    public bool isEmpty;
    public Lore lore;
    public BaseItemData item;
    public Condition condition;
    public Suspect suspect;
    public Ghost ghost;
}
