using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Door : EnvironmentInteractable
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        base.OnEnable();
        
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;

    }

    public override void Interact()
    {
        Debug.Log("HERE!");
        animator.SetTrigger("interactDoor");
    }
}
