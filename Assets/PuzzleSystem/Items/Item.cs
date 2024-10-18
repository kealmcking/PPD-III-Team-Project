using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(EnableInteractUI))]
/// <summary>
/// Represents the physical item to be interacted with. various data types can be added to this so that the data can have a physical representation. 
/// If the item is planned to be unique then you should override this class with the intended item class and its new use functionality
/// </summary>
public class Item : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] BaseItemData data;
    [SerializeField] Rigidbody rb;
    [SerializeField] EnableInteractUI interactUI;
    [SerializeField,Tooltip("Used specifically to handle interactions")] SphereCollider interactCol;
    [SerializeField, Tooltip("Used specifically for turning the collider of the body of the item off and on when using it.")]Collider bodyCol;
    [SerializeField,Tooltip("Only add an override controller here if you want the item to have unique animations when being used")] AnimatorOverrideController overrideController;
    [SerializeField] Transform handlePoint;
    public Transform HandlePoint => handlePoint;
    public BaseItemData Data => data;
    public AnimatorOverrideController OverrideController => overrideController;
    private Guid id = new Guid();
    public Guid ID => id;
    private void Awake()
    {
        
       
        if (interactCol != null)
        {
            interactCol.isTrigger = true;
        }
        else
        {
            Debug.Log("You forgot to add the interact collider");
        }
        rb ??= GetComponent<Rigidbody>();
        interactUI ??= GetComponent<EnableInteractUI>();
        if(bodyCol != null)
        {
            bodyCol.enabled = true;
        }
        else
        {
            Debug.Log("You forgot to add the body collider");
        }
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
    public virtual void Use(){}
    public GameObject GetObject()
    {
        return gameObject;
    }

    public void HandleActivateItemState()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        interactCol.enabled = false;
        interactUI.ToggleCanvas();
        interactUI.enabled = false;
    }
    public void HandleDeactivateItemState()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        interactCol.enabled = true;
        interactUI.ToggleCanvas();
        interactUI.enabled = true;
    }
    public void HandleBodyColliderToggle()
    {
        if (bodyCol != null)
        {
            if (bodyCol.enabled)
                bodyCol.enabled = false;
            else
                bodyCol.enabled = true;
        }
    }
}
