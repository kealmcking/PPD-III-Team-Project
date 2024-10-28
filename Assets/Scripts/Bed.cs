using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : EnvironmentInteractable
{
    Material denyMaterial = null;
    public void Awake()
    {
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            if (mat.name.Contains("OutlineTest"))
            {
                denyMaterial = mat;
                break;
            }

        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 

            if (!TutorialUIManager.Instance.DisplaySleeping)
            {
                TutorialUIManager.Instance.DisplaySleepingTutorial();
            }
            if (!GameManager.instance.isTimeToSleep)
            {
              IsInteractable = false;
              interactUI.ToggleCanvasOff(true);
              denyMaterial.SetColor("_Color", Color.red);
              denyMaterial.SetFloat("_Scale", 1.02f);
            }
            else
            {
             IsInteractable = true;
                interactUI.ToggleCanvasOn();
             denyMaterial.SetColor("_Color", Color.green);
             denyMaterial.SetFloat("_Scale", 1.02f);
            } 
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            denyMaterial.SetFloat("_Scale", 0f);
        }
    }
    public override void Interact()
    {
        if (!GameManager.instance.isTimeToSleep || !IsInteractable) return;
        GameManager.instance.TimeToGoToSleep();         
    }
}
