using System;
using UnityEngine;
/// <summary>
/// Represents the interactable lore around the map. It is case dependent so that every case has a unique lore to it. 
/// </summary>
public class Lore : MonoBehaviour,IInteractable, ICustomizableComponent
{
    [SerializeField, Tooltip("Place a description explaining the lore here")] Description description;
    [SerializeField, Tooltip("represents icon image")] protected Sprite icon;
    private Guid id = new Guid();
    public Guid ID => id;
    public void Interact()
    {
        
    }

    public Payload GetPayload()
    {
        return new Payload { isEmpty = false, lore = this };
    }
}
