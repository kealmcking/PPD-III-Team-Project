using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : EnvironmentInteractable
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !TutorialUIManager.Instance.DisplaySleeping)
        {
                TutorialUIManager.Instance.DisplaySleepingTutorial();
        }
    }
    public override void Interact()
    {
        if (!GameManager.instance.isTimeToSleep) return;
        GameManager.instance.TimeToGoToSleep();         
    }
}
