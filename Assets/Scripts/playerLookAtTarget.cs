using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class playerLookAtTarget : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Transform headBone;
    public Transform headTarget;
    [SerializeField] private float lookSpeed = 5.0f;

    private bool shouldLookAtObject = false;

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            Transform target = headTarget;

            if (target != null)
            {
                Vector3 directionToTarget = target.position - transform.position;
                directionToTarget.y = 0;

                float angle = Vector3.Angle(transform.forward, directionToTarget);

                float maxAngle = 90f;

                if (angle < maxAngle)
                {
                    animator.SetLookAtWeight(1.0f);
                    animator.SetLookAtPosition(target.position);
                }
                else
                {
                    animator.SetLookAtWeight(1.0f);
                    Vector3 forwardLook = transform.position + transform.forward * 2f;
                    forwardLook.y = transform.position.y + 1.5f;
                    animator.SetLookAtPosition(forwardLook);
                }
                
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                headBone.transform.rotation = Quaternion.Slerp(headBone.transform.rotation, targetRotation,
                    Time.deltaTime * lookSpeed);
            }
            else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            Transform targetTransform = GetTargetTransform(other.gameObject);

            if (targetTransform != null)
            {
                shouldLookAtObject = true;
                headTarget = targetTransform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            shouldLookAtObject = false;
            headTarget = null;;
        }
    }

    private Transform GetTargetTransform(GameObject obj)
    {
        Transform headTransform = FindChildWithName(obj.transform, "Head");

        if (headTransform != null)
        {
            return headTransform;
        }

        return obj.transform;
    }

    private Transform FindChildWithName(Transform parent, string childName)
    {
        if (parent.name.Equals(childName, StringComparison.OrdinalIgnoreCase))
        {
            return parent;
        }

        foreach (Transform child in parent)
        {
            Transform result = FindChildWithName(child, childName);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
