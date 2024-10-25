using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using UnityEngine;

public class EnableInteractUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactCanvas;

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
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject canvas in interactCanvas)
        {
            if (canvas == null)
            {
                toRemove.Remove(canvas);
            }
        }

        foreach (GameObject canvas in toRemove)
        {
            interactCanvas.Remove(canvas);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "InteractUI")
            {
                interactCanvas.Add(child.gameObject);
            }

            
        }
        if (interactCanvas != null)
        {
            foreach (GameObject canvas in interactCanvas)
            {
                canvas.SetActive(false);
                canvas.transform.position += new Vector3(0,.2f,.15f);
            }
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
                foreach (GameObject canvas in interactCanvas)
                {
                    if (!canvas.activeSelf)
                    {
                        canvas.SetActive(true);
                    }
                }
                ImInInteractionZone.Invoke(true, this);          
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isInMenu && GetComponent<Suspect>() && interactCanvas != null)
        {
            foreach (GameObject canvas in interactCanvas)
            {
                if (!canvas.activeSelf)
                {
                    canvas.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactCanvas != null)
        {
            foreach (GameObject canvas in interactCanvas)
            {
                canvas.SetActive(false);
            }
            ImInInteractionZone.Invoke(false, this);
        }
    }

    public void ToggleCanvas()
    {
        if (interactCanvas.Count == 0) return;
        foreach (GameObject canvas in interactCanvas)
        {
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
        }

    }

    private void MenuActive(bool context)
    {
        isInMenu = context;
    }
}
