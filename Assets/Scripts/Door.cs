using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource),typeof(Animator),typeof(NavMeshObstacle))]
public class Door : EnvironmentInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip openDoor;
    [SerializeField] AudioClip closeDoor;
    [SerializeField] NavMeshObstacle obstacle;
    private bool isDoorOpen = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.carving = true;
        source.spatialBlend = 1f;
        source.spread = 360f;
        source.maxDistance = 5f;
        source.loop = false;
        source.playOnAwake = false;
    }

    public override void Interact()
    {
        base.Interact();
        animator.SetTrigger("interactDoor");
        source.Play();
        if (isDoorOpen)
        {
            
            source.clip = closeDoor;
            source.Play();
            isDoorOpen = false;
            animator.Play("Close", 0);
        }
        else
        {
          
            source.clip = openDoor; 
            source.Play();
            isDoorOpen = true;
            animator.Play("Open", 0); 
        }
    }
}
