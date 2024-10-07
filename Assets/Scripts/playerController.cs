using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using Input;
using Unity.Mathematics;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController charController;
    [SerializeField] LayerMask ignoreMask;

    public static Action INeedToTurnOffTheInteractUI;

    private Camera _mainCam;

    private float horizInput;
    private float vertInput;

    [Header("Player Stats - Movement Mods")]
    private float _currentSpeed;
    [Range(0.0f, 10.0f)][SerializeField] private float _origSpeed;
    [Range(0.0f, 10.0f)][SerializeField] float _walkSpeed;
    [Range(0.0f, 10.0f)][SerializeField] float _crouchMod;
    [Range(0.0f, 10.0f)][SerializeField] float _climbMod;
    [Range(0.0f, 10.0f)][SerializeField] float _sprintMod;
    [Range(0.0f, 10.0f)][SerializeField] private float _walkBackwardMod;
    [SerializeField] private float moveSmoothSpeed;

    [Header("Player Settings - Rotation Speed")] 
    [SerializeField] private float rotSpeed;

    [Header("Player Stats - Crouching")]
    [SerializeField] private float _crouchTime;
    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _origHeight;
    [SerializeField] private float _newHeight;
    [SerializeField] private Vector3 _origCenter;
    [SerializeField] private Vector3 _newCenter;
    [SerializeField] private Vector3 _crouchCenter;

    [Header("Player Stats - Misc")]
    [SerializeField] float interactDistance;
    [SerializeField] private Light flashlight;
    [SerializeField] private Quaternion itemHandOffset;

    private Condition currentCondition;

    [SerializeField] private Transform handPos;

    Vector3 _moveDir;
    Vector3 _playerVel;
    private float _curVel;

    bool _isFlashlightOn;
    bool _isCrouching;
    bool _isClimbing;
    bool _isFleeing;
    bool _isSprinting;
    
    private void OnEnable()
    {
        InputManager.IHavePressedInteractButton += Interact;
    }

    private void OnDisable()
    {
        InputManager.IHavePressedInteractButton -= Interact;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _walkSpeed;
        _origSpeed = _currentSpeed;
        charController = GetComponent<CharacterController>();
        _mainCam = Camera.main;

        _newCenter = _origCenter;
        _newHeight = _origHeight;
    }

    private void FixedUpdate()
    {
        movement();
        crouch();
        sprint();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDistance, Color.red);
        rotateTowardCamera();
        //toggleFlashlight();
        //updateFlashlightDirection();
    }
    void movement()
    {
        float moveX = InputManager.instance.getMoveAmount().x;
        float moveY = InputManager.instance.getMoveAmount().y;
        Vector3 targetMoveDirection = moveX * transform.right + moveY * transform.forward;

        if (moveY < 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed,_origSpeed * _walkBackwardMod, Time.deltaTime * moveSmoothSpeed);
        }
        else
        {
            if (_isCrouching)
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed,_origSpeed * _crouchMod, Time.deltaTime * moveSmoothSpeed);
            } else if (_isSprinting)
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed,_origSpeed * _sprintMod, Time.deltaTime * moveSmoothSpeed);
            }
            else
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, _origSpeed, Time.deltaTime * moveSmoothSpeed);
            }
        }
        
        _moveDir = Vector3.Lerp(_moveDir, targetMoveDirection, 0.1f);
        charController.Move(_moveDir * _currentSpeed * Time.deltaTime);
    }

    void rotateTowardCamera()
    {
        if (_moveDir.magnitude > 0.1f)
        {
            Vector3 cameraForward = _mainCam.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }
    }

    void sprint()
    {
        if (InputManager.instance.getSprintHeld())
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
    }

    void crouch()
    {
        if (InputManager.instance.getIsCrouch())
        {
            if (!_isCrouching)
            {
                _newHeight = _crouchHeight;
                _newCenter = _crouchCenter;
                _isCrouching = true;
            }
        }
        else
        {
            if (_isCrouching)
            {
                _newHeight = _origHeight;
                _newCenter = _origCenter;
                _isCrouching = false;
            }
        }
        charController.height = Mathf.Lerp(charController.height, _newHeight, Time.deltaTime / _crouchTime);
        charController.center = Vector3.Lerp(charController.center, _newCenter, Time.deltaTime / _crouchTime);
    }

    void toggleFlashlight()
    {
        flashlight.enabled = InputManager.instance.getFlashlight();
    }
    void updateFlashlightDirection()
    {
        if (flashlight.enabled)
        {
            flashlight.transform.rotation = Camera.main.transform.rotation;
        }
    }
    
    private void Interact(EnableInteractUI interactUI)
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactDistance);

        
        foreach (var collider in colliders)
        {
            IInteractable interactable;
            collider.gameObject.TryGetComponent(out interactable);
            
            if (interactable == null) continue;
            Payload payload = interactable.GetPayload();
            interactable.Interact();
            
            
            switch (interactable)
            {
                case Item item:
                    // Do item stuff
                    // - Send to inventory
                    break;
                case Lore lore:
                    lore.Interact();
                    break;
                case DialogueManager dialogue:
                    // - Send to dialogue UI
                    dialogue.enableDialogueUI(dialogue.SuspectBeingInteractedWith());
                    break;
                case Condition condition:
                    if (condition.CanPickup())
                    {
                        currentCondition = condition;
                        condition.gameObject.transform.SetParent(handPos);
                        condition.transform.localPosition = Vector3.zero;
                        condition.transform.localRotation = Quaternion.identity * itemHandOffset;
                        interactUI.ToggleCanvas();
                    }
                    break;
            }
        }
        
        
    }

    private void ThrowObject()
    {
        if (currentCondition == null) return;
        
        currentCondition.gameObject.transform.parent = null;


        // Throw object at target
    }
}