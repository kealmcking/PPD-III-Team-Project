using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController charController;
    [SerializeField] LayerMask ignoreMask;

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

    [Header("Player Stats - Crouching")]
    [SerializeField] private float _crouchTime;
    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _origHeight;
    [SerializeField] private float _newHeight;

    [Header("Player Stats - Misc")]
    [SerializeField] float interactDistance;
    [SerializeField] private Light flashlight;

    Vector3 _moveDir;
    Vector3 _playerVel;
    private float _curVel;

    bool _isFlashlightOn;
    bool _isCrouching;
    bool _isClimbing;
    bool _isFleeing;
    bool _isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _origSpeed;
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDistance, Color.red);
        movement();
        crouch();
        sprint();
        toggleFlashlight();
        updateFlashlightDirection();
    }
    void movement()
{
        _moveDir = InputManager.instance.getMoveAmount().x * transform.right 
                   + InputManager.instance.getMoveAmount().y * transform.forward;
        charController.Move(_moveDir * _currentSpeed * Time.deltaTime);
    }

    void sprint()
    {
        if (InputManager.instance.getSprintHeld())
        {
            if (!_isSprinting)
            {
                _currentSpeed = _origSpeed * _sprintMod;
            }
            _isSprinting = true;
        }
        else if (!InputManager.instance.getSprintHeld())
        {
            _currentSpeed = _origSpeed;
            _isSprinting = false;
        }
    }

    void crouch()
    {
        if (InputManager.instance.getIsCrouch())
        {
            _currentSpeed = _origSpeed * _crouchMod;
            _newHeight = _crouchHeight;
            _isCrouching = true;
        }
        else if (!InputManager.instance.getIsCrouch())
        {
            _currentSpeed = _origSpeed;
            _newHeight = _origHeight;
            _isCrouching = false;
        }
        charController.height = Mathf.Lerp(charController.height, _newHeight, Time.deltaTime / _crouchTime);
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
}