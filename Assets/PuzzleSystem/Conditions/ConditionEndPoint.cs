using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
