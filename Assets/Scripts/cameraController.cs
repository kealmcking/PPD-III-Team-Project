using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class CameraController : MonoBehaviour
{
    [Header("Camera - Sensitivity")]
    public float sensitivity = 100f;
    [SerializeField] private int lockVertMin = -60;
    [SerializeField] private int lockVertMax = -60;
    [SerializeField] private bool invertY;

    [Header("Camera - Player Ref")]
    private float rotX;
    private Transform player;

    void Start()
    {
        player = transform.parent;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        cameraMovement();
    }

    void cameraMovement()
    {
        float mouseY = InputManager.instance.getAimAmount().y * sensitivity * Time.deltaTime;
        float mouseX = InputManager.instance.getAimAmount().x * sensitivity * Time.deltaTime;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);
        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}