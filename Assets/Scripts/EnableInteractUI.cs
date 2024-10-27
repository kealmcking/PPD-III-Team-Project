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

    private bool isInMenu;
    

    private void OnEnable()
    {
        playerController.INeedToTurnOffTheInteractUI += ToggleCanvas;
        DialogueManager.DialogueMenuActive += MenuActive;
    }

    private void OnDisable()
    {
        playerController.INeedToTurnOffTheInteractUI -= ToggleCanvas;
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
            Renderer parentRenderer = GetComponent<Renderer>();
            Vector3 parentCenter = transform.position; // Default to parent position

            if (parentRenderer != null)
            {
                parentCenter = parentRenderer.bounds.center;
                interactCanvas.transform.position = parentCenter;
                interactCanvas.transform.position += new Vector3(0, .5f, .2f);
            }
            else
            {
                interactCanvas.transform.position += new Vector3(0, 1.8f, 0);
            }
           
            
            interactCanvas.SetActive(false);             
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactCanvas!= null)
        {
            if (!interactCanvas.activeSelf)
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
            if (!interactCanvas.activeSelf)
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

    public void ToggleCanvas()
    {
        if (interactCanvas == null) return;
        if (interactCanvas.activeSelf)
        {
            interactCanvas.SetActive(false);
        }

    }

    private void MenuActive(bool context)
    {
        isInMenu = context;
    }
}
