using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class EnvironmentInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected EnableInteractUI interactUI;
    [SerializeField] Collider col;
    public virtual void OnEnable()
    {
        if (col == null)
        {
            col = GetComponent<SphereCollider>();
        }

        col.isTrigger = true;

        if (interactUI == null)
        {
            interactUI = GetComponent<EnableInteractUI>();
        }
    }
    public GameObject GetObject()
    {
       return gameObject;
    }

    public virtual void Interact()
    {
        
    }
}
