using System;
using UnityEngine;

/// <summary>
/// Represents the physical item to be interacted with. various data types can be added to this so that the data can have a physical representation. 
/// </summary>
public class Item : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] protected BaseItemData data;
    public BaseItemData Data => data;
    public virtual void Interact()
    {
        Destroy(gameObject, .5f);
    }
    public Payload GetPayload()
    {
        return new Payload { isEmpty = false, item = data };
    }
}
