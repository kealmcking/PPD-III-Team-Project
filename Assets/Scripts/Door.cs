using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : EnvironmentInteractable
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        animator.SetTrigger("interactDoor");
    }
}
