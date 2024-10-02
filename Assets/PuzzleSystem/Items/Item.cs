using System;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] protected BaseItemData data;
    public static Action<BaseItemData> SendItem;

    public virtual void Interact()
    {
        SendItem.Invoke(data);
        Destroy(gameObject, .5f);
    }
}
