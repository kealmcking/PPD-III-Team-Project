using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider), typeof(EnableInteractUI))]
public class Detective : EnvironmentInteractable
{
    public override void Interact()
    {
        Input.InputManager.instance.DisableCharacterInputs();
        if (GameManager.instance.MenuActive != null)
        {
            GameManager.instance.DeactivatePauseMenu();
            GameManager.instance.UnpauseGame();
        }
        if (GameManager.instance.InventoryActive && GameManager.instance.MenuActive != GameManager.instance.MenuInventory)
            GameManager.instance.DeactivateInventoryUISecondary();
        else if (GameManager.instance.InventoryActive) GameManager.instance.DeactivateInventoryUI();

        if (GameManager.instance.CraftTableActive) GameManager.instance.DeactivateCraftTableUI();
        interactUI.ToggleCanvasOff();
        GameManager.instance.ActivateDecisionUI();
    }
}
