using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider),  typeof(EnableInteractUI))]

public class WorkBench : EnvironmentInteractable
{
   
  

    public override void Interact()
    { 
        Input.InputManager.instance.DisableCharacterInputs();
        if(GameManager.instance.MenuActive != null)
        {
            GameManager.instance.DeactivatePauseMenu();
            GameManager.instance.UnpauseGame();
        }
        if (!GameManager.instance.InventoryActive)
        {
            GameManager.instance.ActivateInventoryUISecondary();
        }
        interactUI.ToggleCanvas();
        GameManager.instance.ActivateCraftTableUI();
    }

   
}
