using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using Input;
using Unity.Mathematics;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController charController;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private playerLookAtTarget playerLookAtTarget;
    public List<SkinnedMeshRenderer> playerModels = new List<SkinnedMeshRenderer>();

    public static Action INeedToTurnOffTheInteractUI;

    private Camera _mainCam;
    private audioManager audioManager;

    private float horizInput;
    private float vertInput;

    [Header("Player Settings - Selected Character")]
    [SerializeField] private CharacterDB characterDB;
    [SerializeField] public Character _character;
    private GameObject _currentCharacterModel;

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

    [Header("Player Stats - Gravity")]
    [SerializeField] private float gravity = -9.81f;


    [SerializeField] private Transform handPos;
    private IInteractable objectInHand;
    private bool hasTurned;
    private bool startTurning;

    Vector3 _moveDir;
    Vector3 _playerVel;
    private float _curVel;

    private float targetVertValue;

    bool _isFlashlightOn;
    bool _isCrouching;
    bool _isClimbing;
    bool _isFleeing;
    bool _isSprinting;


    private void OnEnable()
    {
        EventSheet.IHavePressedInteractButton += Interact;
    }

    private void OnDisable()
    {
        EventSheet.IHavePressedInteractButton -= Interact;
    }

    private void Awake()
    {
         audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<audioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        int selectedOption = PlayerPrefs.GetInt("selectedOption", 0);
        _currentSpeed = _walkSpeed;
        _origSpeed = _currentSpeed;
        charController = GetComponent<CharacterController>();
        _mainCam = Camera.main;

        _newCenter = _origCenter;
        _newHeight = _origHeight;

        _playerVel = Vector3.zero;
    }
    public void SetCharacterModel(GameObject characterModel)
    {
        _currentCharacterModel = characterModel;
    }
    private void FixedUpdate()
    {
        movement();
        crouch();
    }

    // Update is called once per frame
    private void LateUpdate()
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

        horizInput = moveX;
        vertInput = moveY;
        Vector3 targetMoveDirection = moveX * transform.right + moveY * transform.forward;

        // Base speed
        float targetSpeed = _origSpeed;

        // Check if walking backward
        if (moveY <= -0.1f)
        {
            targetSpeed *= _walkBackwardMod;

        }

        // Check if crouching
        if (_isCrouching)
        {
            targetSpeed *= _crouchMod;
        }
        // Check if sprinting
        else if (_isSprinting)
        {
            targetSpeed *= _sprintMod;
        }

        // Smoothly transition to the target speed
        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * moveSmoothSpeed);

        // Apply movement
        _moveDir = Vector3.Lerp(_moveDir, targetMoveDirection, 0.1f);
        charController.Move(_moveDir * _currentSpeed * Time.deltaTime);

        sprint();

        float currentVertValue = _animator.GetFloat("vert");
        float newVertValue = Mathf.Lerp(currentVertValue, targetVertValue, Time.deltaTime * moveSmoothSpeed);

        // Update animator parameters
        _animator.SetFloat("horiz", moveX);
        _animator.SetFloat("vert", newVertValue);

        ApplyGravity();
    }

    private void footStep()
    {
        if (!_isCrouching && !_isSprinting)          //Walking
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepWalkVol);
        }
        else if (!_isCrouching && _isSprinting)   //Sprinting
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepRunVol);
        }
        else if (!_isSprinting && _isCrouching)   //Crouching
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepCrouchVol);
        }
    }

    private void ApplyGravity()
    {
        if (charController.isGrounded)
        {
            _playerVel.y = 0;
        }
        else
        {
            _playerVel.y += gravity * Time.deltaTime;
        }

        charController.Move(_playerVel * Time.deltaTime);
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
        if (InputManager.instance.getSprintHeld() && vertInput > 0)
        {
            _isSprinting = true;
            targetVertValue = 2f;
        }
        else
        {
            _isSprinting = false;
            targetVertValue = Mathf.Clamp(vertInput, -1f, 1f);
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

                audioManager.PlaySFX(audioManager.crouchDown[UnityEngine.Random.Range(0, audioManager.crouchDown.Length)], audioManager.crouchVol);
            }
        }
        else
        {
            if (_isCrouching)
            {
                _newHeight = _origHeight;
                _newCenter = _origCenter;
                _isCrouching = false;

                audioManager.PlaySFX(audioManager.crouchUp[UnityEngine.Random.Range(0, audioManager.crouchUp.Length)], audioManager.crouchVol);
            }
        }

        _animator.SetBool("isCrouched", _isCrouching);
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

    private void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactDistance);

        float closestDistanceSqr = float.MaxValue; // Store the closest object's squared distance
        Collider closestCollider = null;

        foreach (var collider in colliders)
        {
            // Calculate the squared distance to avoid unnecessary square root operations
            float distSqr = (collider.transform.position - transform.position).sqrMagnitude;

            // Find the nearest object
            if (distSqr < closestDistanceSqr)
            {
                // Check the angle to ensure it's within 45 degrees
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle > 45f) continue;

                // Check if nothing is blocking the object using a Raycast
                if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, Mathf.Sqrt(distSqr)))
                {
                    // Ensure the hit object is the same as the collider
                    if (hit.collider == collider)
                    {
                        closestDistanceSqr = distSqr;
                        closestCollider = collider;
                    }
                }
            }
        }

        // Interact with the closest valid object
        if (closestCollider != null && closestCollider.TryGetComponent(out IInteractable interactable))
        {
            _animator.SetTrigger("activate");
            interactable.Interact();

            if (interactable is Condition condition && condition.CanPickup() && !condition.HasBeenPickedUp())
            {
                AddObjectToRightHand(condition);
                playerLookAtTarget.headTarget = null;
            }
        }
    }

    private void AddObjectToRightHand(IInteractable obj)
    {
        obj.GetObject().transform.SetParent(handPos);
        obj.GetObject().transform.localPosition = Vector3.zero;
        obj.GetObject().transform.localRotation = Quaternion.identity * itemHandOffset;
        objectInHand = obj;
    }

    private void ThrowObject()
    {
        if (objectInHand == null) return;
        objectInHand.GetObject().transform.SetParent(null);
        _animator.SetTrigger("activate");
        Item item = objectInHand.GetObject().GetComponent<Item>();
        if (item != null)
        {
            item.ItemPulse(transform.forward);
        }        
    }

    public void UpdatePlayerCharacter(int index)
    {
        playerModels[index].enabled = true;
    }
}