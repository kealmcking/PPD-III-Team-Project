using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InteractableUIElement : MonoBehaviour
{

    [SerializeField] private Sprite pcButton_spr, xboxButton_spr, psButton_spr, switchButton_spr;
    [SerializeField] private Image uiElement;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color color;

    [SerializeField] private bool isDialogueUI;
    
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
            if (pcButton_spr != null)
            {
                if (isDialogueUI)
                {
                    uiElement.enabled = true;
                }
                else
                {
                    uiElement.color = color;
                }
            }
            else
            {
                uiElement.enabled = false;
            }
            
            if (text != null)
            {
                text.enabled = true;
                text.text = "E";
            }
            
        } else if (device is Gamepad gamepad)
        {
            if (gamepad.displayName.Contains("Xbox"))
            {
                uiElement.enabled = true;
                uiElement.color = color;
                
                if (text != null)
                {
                    text.enabled = false;
                    text.text = "";
                }
                
                uiElement.sprite = xboxButton_spr;
            } else if (gamepad.displayName.Contains("Playstation"))
            {
                uiElement.enabled = true;
                uiElement.color = color;
                
                if (text != null)
                {
                    text.enabled = false;
                    text.text = "";
                }
                
                uiElement.sprite = psButton_spr;
            } else if (gamepad.displayName.Contains("Switch"))
            {
                uiElement.enabled = true;
                uiElement.color = color;
                
                if (text != null)
                {
                    text.enabled = false;
                    text.text = "";
                }
                
                uiElement.sprite = switchButton_spr;
            }
            else
            {
                uiElement.enabled = true;
                uiElement.color = color;
                
                if (text != null)
                {
                    text.enabled = false;
                    text.text = "";
                }
            }
        }
    }
}
