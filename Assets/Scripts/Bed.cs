using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : EnvironmentInteractable
{
    public override void Interact()
    {
        if (!GameManager.instance.isTimeToSleep) return;
        GameManager.instance.wentToSleep = true;
        GameManager.instance.TimeToGoToSleep(); 
        
    }
}
