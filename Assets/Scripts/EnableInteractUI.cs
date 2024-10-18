using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class EnableInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactCanvas;

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

    // Start is called before the first frame update
    void Start()
    {
        if (interactCanvas != null)
        {
            interactCanvas = GetComponentInChildren<Canvas>().gameObject;
            interactCanvas.SetActive(false);
        }
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactCanvas.SetActive(true);
            ImInInteractionZone.Invoke(true, this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isInMenu && GetComponent<Suspect>())
        {
            interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactCanvas.SetActive(false);
            ImInInteractionZone.Invoke(false, this);
        }
    }

    public void ToggleCanvas()
    {
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
