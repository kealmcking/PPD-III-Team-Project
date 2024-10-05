using System;
using UnityEngine;

/// <summary>
/// Represents the physical item to be interacted with. various data types can be added to this so that the data can have a physical representation. 
/// </summary>
public class Item : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] protected BaseItemData data;
    public BaseItemData Data => data;
    public static Action<IInteractable> SendItem;  
    public virtual void Interact()
    {
        SendItem.Invoke(this);
        Destroy(gameObject, .5f);
    }
    public Payload GetPayload()
    {
        return new Payload { isEmpty = false, item = data };
    }
}
