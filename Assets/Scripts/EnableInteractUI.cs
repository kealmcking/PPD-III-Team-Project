using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        interactCanvas = GetComponentInChildren<Canvas>().gameObject;
        interactCanvas.SetActive(false);
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactCanvas.SetActive(false);
        }
    }
}
