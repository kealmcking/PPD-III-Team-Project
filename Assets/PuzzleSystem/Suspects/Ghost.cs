using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour, IInteractable, ICustomizableComponent
{
    SuspectData suspectData;
    [SerializeField] GhostData ghostData;
    public GhostData GhostData => ghostData;
    public SuspectData SuspectData { get { return suspectData; } set { suspectData = value; } }

    [SerializeField] EnableInteractUI interactUI;
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

        interactUI.ToggleCanvas();
    }

 
}
