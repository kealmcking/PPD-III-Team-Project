using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class InteractableGlow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.TryGetComponent(out IInteractable interactable))
        {
            Debug.Log("Collided with interactable");
            Renderer renderer = interactable.GetObject().GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log("Setting material");
                Material material = renderer.materials[1];
                if (material != null)
                    material.SetFloat("_Scale", 1.02f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
    
        if (other.TryGetComponent(out IInteractable interactable))
        {
            Debug.Log("leaving interactable");
            Renderer renderer = interactable.GetObject().GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log("Setting material");
                Material material = renderer.materials[1];
                if (material != null)
                material.SetFloat("_Scale", 0f);
            }
        }
    }
}
