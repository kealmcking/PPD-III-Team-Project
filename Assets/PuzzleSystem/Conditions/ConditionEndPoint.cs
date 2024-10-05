using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents an end point or goal for a puzzle. 
/// for example can be used as the goal the player needs to push an object to or
/// for them to place an object on.
/// </summary>
[RequireComponent(typeof(SphereCollider),typeof(Rigidbody))]
public class ConditionEndPoint : MonoBehaviour
{
    [SerializeField] SphereCollider col;
    [SerializeField] Rigidbody rb;
    [SerializeField] float colRadius;
    private void Awake()
    {
        col ??= GetComponent<SphereCollider>();  
        rb ??= GetComponent<Rigidbody>();

        col.isTrigger = true;
        col.radius = colRadius;

        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
