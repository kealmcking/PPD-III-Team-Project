using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
/// <summary>
/// Allows a simple way to track the suspects who are in the game and whether or not they are the killer. 
/// </summary>
public class Suspect : MonoBehaviour, IInteractable
{
    [SerializeField] string suspectName;
    [SerializeField] Sprite icon; 
    [SerializeField] NPC npc;
    [SerializeField] bool isBeingInteractedWith;
    [SerializeField] SuspectData data;
    [SerializeField] GameObject mask;
    public string Name => suspectName;  
    public Sprite Icon => icon;
    public NPC Npc => npc;
    public bool IsBeingInteractedWith
    {
        get => isBeingInteractedWith;
        set => isBeingInteractedWith = value;
    }

    public void Start()
    {
        DialogueManager.instance.AddSuspect(this);
    }

    public Payload GetPayload()
    {
        return new Payload() {isEmpty = true};
    }

    public void Interact()
    {
        // Do interaction stuff here
    }

    Guid id = new Guid();
    public SuspectData Data => data;
    public bool IsKiller { get; set; } = false;
    public Guid ID => id;
    public GameObject Mask => mask;
    public void Awake()
    {
        //mask = transform.Find("Mask").gameObject;
        if (mask == null)
        {
            Debug.LogError("You do not have a mask component on this suspect: " + name + ", add a mask before continuing");
        }
    }
    public void ActivateMask()
    {
        if (mask != null)
        {
            mask.SetActive(true);
        }
    }


}
