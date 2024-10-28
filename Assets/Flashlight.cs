using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private Light pointLight;
    [SerializeField] private bool flashlightStatus = true;
    [SerializeField] private MeshRenderer renderer;

    [SerializeField] private Material emissiveMat, nonEmissiveMat;
    
    private void Update()
    {
        if (InputManager.instance.flashlightAction.WasPressedThisFrame())
        {
            Material[] materials = renderer.materials;

            materials[0] = flashlightStatus ? nonEmissiveMat : emissiveMat;
            renderer.materials = materials;
            
            light.enabled = !flashlightStatus;
            pointLight.enabled = !flashlightStatus;
            flashlightStatus = !flashlightStatus;

        }
    }
}
