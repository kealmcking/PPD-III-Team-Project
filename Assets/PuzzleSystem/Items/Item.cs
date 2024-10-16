using System;
using UnityEngine;
[RequireComponent(typeof(SphereCollider), typeof(Rigidbody),typeof(EnableInteractUI))]
/// <summary>
/// Represents the physical item to be interacted with. various data types can be added to this so that the data can have a physical representation. 
/// </summary>
public class Item : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] BaseItemData data;
    [SerializeField] Rigidbody rb;
    [SerializeField] EnableInteractUI interactUI;
    [SerializeField] Collider col;
    public BaseItemData Data => data;
    private Guid id = new Guid();
    public Guid ID => id;
    private void Awake()
    {
        col ??= GetComponent<SphereCollider>();
        col.isTrigger = true;
        rb ??= GetComponent<Rigidbody>();
        interactUI ??= GetComponent<EnableInteractUI>();
    }
    public void ItemPulse(Vector3 startPosition)
    {
        rb.AddForce(startPosition * 5, ForceMode.Impulse);
    }
    public void Interact()
    {
        interactUI.ToggleCanvas();
        EventSheet.SendItemToInventory?.Invoke(this);
    }
    public GameObject GetObject()
    {
        return gameObject;
    }
}
