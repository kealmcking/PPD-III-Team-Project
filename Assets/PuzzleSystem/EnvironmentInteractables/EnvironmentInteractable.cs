using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider),typeof(EnableInteractUI))]
public abstract class EnvironmentInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected EnableInteractUI interactUI;
    [SerializeField] SphereCollider col;
    public virtual void OnEnable()
    {
        if (col == null)
        {
            col = GetComponent<SphereCollider>();
        }

        col.isTrigger = true;
        col.radius = 1.5f;
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
        interactUI.ToggleCanvas();
    }
}
