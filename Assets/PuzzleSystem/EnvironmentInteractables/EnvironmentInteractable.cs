using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteractable : MonoBehaviour,IInteractable
{
    [SerializeField] protected EnableInteractUI interactUI;
    [SerializeField] Collider col;
    public virtual void Awake()
    {
        col ??= GetComponent<SphereCollider>();
        col.isTrigger = true;
        interactUI ??= GetComponent<EnableInteractUI>();
    }
    public GameObject GetObject()
    {
       return gameObject;
    }

    public virtual void Interact()
    {
        
    }

  
}
