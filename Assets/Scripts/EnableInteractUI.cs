using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using UnityEngine;

public class EnableInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactCanvas;
    private Material glowMaterial;
    public static Action<bool, EnableInteractUI> ImInInteractionZone;
    bool isCanvasOffManually = false;
    private bool isInMenu;
    public GameObject InteractCanvas => interactCanvas;

    private void OnEnable()
    {
        playerController.INeedToTurnOffTheInteractUI += ToggleCanvasOff;
        DialogueManager.DialogueMenuActive += MenuActive;
    }

    private void OnDisable()
    {
        playerController.INeedToTurnOffTheInteractUI -= ToggleCanvasOff;
        DialogueManager.DialogueMenuActive -= MenuActive;
    }

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("InteractUI"))
            {
                interactCanvas = child.gameObject;
                break;
            }


        }
    }

    void Start()
    {
       
     
        if (interactCanvas != null)
        {
            interactCanvas.transform.position = transform.position;
            Renderer renderer = GetComponent<Renderer>();
            Vector3 parentCenter = transform.position; // Default to parent position

            if (renderer != null)
            {
                parentCenter = renderer.bounds.center;
                interactCanvas.transform.position = parentCenter;
                interactCanvas.transform.position += new Vector3(0, 0.5f, 0) + transform.forward * 0.3f;
            }
            else
            {
                interactCanvas.transform.position += new Vector3(0, 1.8f, 0) + transform.forward * 0.3f;
            }
           
            
            interactCanvas.SetActive(false);             
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactCanvas!= null)
        {
            if (!interactCanvas.activeSelf && !isCanvasOffManually)
            {
                interactCanvas.SetActive(true);
            }

            ImInInteractionZone.Invoke(true, this);          
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isInMenu && GetComponent<Suspect>() && interactCanvas != null)
        {
            if (!interactCanvas.activeSelf && !isCanvasOffManually)
            {
                interactCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactCanvas != null)
        {
            if (interactCanvas.activeSelf)
            {
                interactCanvas.SetActive(false);
            }
            ImInInteractionZone.Invoke(false, this);
        }
    }

    public void ToggleCanvasOff(bool setCanvasOffManually = false)
    {
        if (interactCanvas == null) return;
            interactCanvas.SetActive(false);
        if (setCanvasOffManually) isCanvasOffManually = true;
    }
    public void ToggleCanvasOn()
    {
        if (interactCanvas == null) return;
        if (isCanvasOffManually) isCanvasOffManually = false;
    }
    private void MenuActive(bool context)
    {
        isInMenu = context;
    }
}
