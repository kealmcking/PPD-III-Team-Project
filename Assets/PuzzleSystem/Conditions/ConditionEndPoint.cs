using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents an end point or goal for a puzzle. 
/// for example can be used as the goal the player needs to push an object to or
/// for them to place an object on.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ConditionEndPoint : MonoBehaviour
{
    [SerializeField] SphereCollider col;

    [SerializeField] float colRadius;
    [SerializeField] Animator anim;
    public Animator Anim => anim;
    private void Awake()
    {
        col ??= GetComponent<SphereCollider>();  
      

        anim??= GetComponent<Animator>();
    }
}
