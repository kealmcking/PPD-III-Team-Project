using DialogueSystem;
using Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Allows a simple way to track the suspects who are in the game and whether or not they are the killer. 
/// </summary>
[RequireComponent(typeof(SphereCollider), typeof(EnableInteractUI),typeof(Animator))]
public class Suspect : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] SuspectData data;
    [SerializeField] GameObject mask;
    public bool IsInteractable { get; set; } = true;
    [SerializeField] EnableInteractUI interactUI;
    [SerializeField] Collider col;
    [SerializeField] Animator anim;
    //add weapon as well to activate when revealing the killer
    Guid id = new Guid();
    public Guid ID => id;
    public SuspectData Data => data;
    public Animator Anim => anim;
    public bool IsKiller = false;
    //public bool IsKiller { get; set; } = false;

   
    public GameObject Mask => mask;
    private void Awake()
    {
        col ??= GetComponent<SphereCollider>();
        col.isTrigger = true;
        /* mask = transform.Find("Mask").gameObject;
         if(mask == null)
         {
             Debug.LogError("You do not have a mask component on this suspect: " +name+", add a mask before continuing");
         }*/
        IsKiller = false;
    }

    public void ActivateMask()
    {
        if (mask != null)
        {
            mask.SetActive(true);
        }
    }
    public virtual void Interact()
    {
        interactUI.ToggleCanvasOff();
        DialogueManager.instance.enableDialogueUI(this);     
    }
    public GameObject GetObject()
    {
        return gameObject;
    }

}