using System;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] protected BaseItemData data;
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
