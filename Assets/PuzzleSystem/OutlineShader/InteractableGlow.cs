using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableGlow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if(interactable != null)
            {
              
                Renderer renderer = interactable.GetObject().GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material matCopy;
                    foreach(var mat in renderer.materials)
                    {
                        matCopy = mat;
                        if (matCopy.name.Contains("OutlineTest"))
                        {
                            matCopy.SetFloat("_Scale", 1.02f);
                        }
                    } 
                }
            }         
        }
    }
    private void OnTriggerExit(Collider other)
    {
    
        if (other.TryGetComponent(out IInteractable interactable))
        {
            Renderer renderer = interactable.GetObject().GetComponent<Renderer>();
            if (renderer != null)
            {
                Material matCopy;
                foreach (var mat in renderer.materials)
                {
                    matCopy = mat;
                    if (matCopy.name.Contains("OutlineTest"))
                    {
                        matCopy.SetFloat("_Scale", 0f);
                    }
                }
            }
        }
    }
}
