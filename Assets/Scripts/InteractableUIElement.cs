using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InteractableUIElement : MonoBehaviour
{

    [SerializeField] private Sprite pcButton_spr, xboxButton_spr, psButton_spr, switchButton_spr;
    [SerializeField] private Image uiElement;
    
    // Start is called before the first frame update
    void Start()
    {
        uiElement = GetComponent<Image>();
        DetectChangeInDevice();
    }

    private void Update()
    {
        var activeDevice = InputSystem.GetDevice<InputDevice>();
        ChangeElement(activeDevice);
    }

    void DetectChangeInDevice()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed)
            {
                ChangeElement(device);
            }
        };
    }

    void ChangeElement(InputDevice device)
    {
        if (device is Keyboard || device is Mouse)
        {
            uiElement.sprite = pcButton_spr;
        } else if (device is Gamepad gamepad)
        {
            if (gamepad.displayName.Contains("Xbox"))
            {
                uiElement.sprite = xboxButton_spr;
            } else if (gamepad.displayName.Contains("Playstation"))
            {
                uiElement.sprite = psButton_spr;
            } else if (gamepad.displayName.Contains("Switch"))
            {
                uiElement.sprite = switchButton_spr;
            }
            else
            {
                uiElement.sprite = pcButton_spr;
            }
        }
    }
}