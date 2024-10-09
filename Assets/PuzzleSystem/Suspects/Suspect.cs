using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Allows a simple way to track the suspects who are in the game and whether or not they are the killer. 
/// </summary>
public class Suspect : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] SuspectData data;
    [SerializeField] GameObject mask;
    [SerializeField] bool isBeingInteractedWith;

    Guid id = new Guid();
    public SuspectData Data => data;
    public bool IsKiller { get; set; } = false;
    public bool IsBeingInteractedWith => isBeingInteractedWith;
    public Guid ID => id;
    public GameObject Mask => mask;
    public void Awake()
    {
        mask = transform.Find("Mask").gameObject;
        if(mask == null)
        {
            Debug.LogError("You do not have a mask component on this suspect: " +name+", add a mask before continuing");
        }
    }
    public void ActivateMask()
    {
        if(mask != null)
        {
            mask.SetActive(true);
        }
    }
    public void Interact()
    {

    }
    public Payload GetPayload()
    {
        return new Payload{ isEmpty = false, suspect = this};
    }

}


