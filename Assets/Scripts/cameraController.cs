using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class CustomCameraController : MonoBehaviour
{
    public Transform player;                                        // Reference to the player's transform
    public Vector3 offset = new Vector3(0.5f, 1.5f, -3f);     // Camera offset (with x for shoulder effect)
    public float smoothSpeed = 0.125f;                              // Smoothing speed for camera movement
    public float sensitivity = 100f;                                // Sensitivity for manual camera control
    public LayerMask collisionLayers;                               // Layers to check for camera collision
    public float collisionBuffer = 0.2f;                            // Buffer distance for collision
    public Camera cameraComponent;                                  // Reference to the camera component
    public float normalFOV = 60f;                                   // Normal field of view
    public float sprintFOV = 75f;                                   // Field of view during sprint
    public float fovSpeed = 2f;                                     // Speed for FOV transition

    private float verticalRotation = 0f;                            // To track vertical (pitch) rotation
    private float horizontalRotation = 0f;                          // To track horizontal (yaw) rotation

    private Vector2 smoothedMouseDelta;
    private float mouseSmoothing = 0.1f;

    void Start()
    {
        // Initialize the horizontal rotation to match the player's initial facing direction
        horizontalRotation = player.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        HandleCameraRotation();
        HandleCameraMovement();
        HandleCameraCollision();
    }

    // Allow full camera rotation with the mouse or joystick
    private void HandleCameraRotation()
    {
        Vector2 mouseDelta = InputManager.instance.getAimAmount() * (sensitivity * Time.deltaTime);
        smoothedMouseDelta = Vector2.Lerp(smoothedMouseDelta, mouseDelta, mouseSmoothing);
        
        float inX = smoothedMouseDelta.x;
        float inY = smoothedMouseDelta.y;

        // Update the horizontal and vertical rotation based on input
        horizontalRotation += inX;
        verticalRotation -= inY;
        verticalRotation = Mathf.Clamp(verticalRotation, -30f, 60f);  // Limit vertical rotation (pitch)

        // Calculate the new camera rotation based on horizontal and vertical input
        Quaternion rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        transform.rotation = rotation;
        
    }

    // Smoothly follow the player while keeping the offset and rotation in place
    private void HandleCameraMovement()
    {
        // Apply the offset in the local space to correctly use the x offset for side shift (shoulder view)
        Vector3 desiredPosition = player.position 
                                  + transform.right * offset.x   // Use the camera's right vector for x offset (side shift)
                                  - transform.forward * offset.z  // Use the camera's forward vector for depth
                                  + Vector3.up * offset.y;        // Add the height

        // Smooth the camera movement using Vector3.Lerp
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    // Adjust the camera's position to avoid clipping through objects
    private void HandleCameraCollision()
    {
        Vector3 desiredPosition = player.position 
                                  + transform.right * offset.x
                                  - transform.forward * offset.z 
                                  + Vector3.up * offset.y;
                                  
        RaycastHit hit;
        if (Physics.Linecast(player.position, desiredPosition, out hit, collisionLayers))
        {
            // Move the camera closer if there's an obstacle
            desiredPosition = hit.point + (hit.normal * collisionBuffer);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}