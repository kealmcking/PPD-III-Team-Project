using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using Input;
using Unity.Mathematics;
//using static UnityEditor.Progress;

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
    private RuntimeAnimatorController originalController;
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

    bool _isCrouching;
    bool _isClimbing;
    bool _isFleeing;
    bool _isSprinting;
    public bool isDead = false;

    private int selectedOption;



    public void killPlayer()
    {
            GameManager.instance.LoseGame();
    }

    private void OnEnable()
    {
        EventSheet.IHavePressedInteractButton += Interact;
        EventSheet.EquipItem += ValidateEquippedItem;
        EventSheet.UseHeldItem += Use;
        EventSheet.DropHeldItem += Drop;
        EventSheet.ThrowHeldItem += Throw;
        EventSheet.ThrowAnimationEvent += ThrowEvent;
        EventSheet.ItemColliderAnimationEvent += ItemColliderToggle;
    }

    private void OnDisable()
    {
        EventSheet.IHavePressedInteractButton -= Interact;
        EventSheet.EquipItem -= ValidateEquippedItem;
        EventSheet.UseHeldItem -= Use;
        EventSheet.DropHeldItem -= Drop;
        EventSheet.ThrowHeldItem -= Throw;
        EventSheet.ThrowAnimationEvent -= ThrowEvent;
        EventSheet.ItemColliderAnimationEvent -= ItemColliderToggle;
    }

    private void Awake()
    {
         originalController = _animator.runtimeAnimatorController;
         audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<audioManager>();
         selectedOption = PlayerPrefs.GetInt("selectedOption", 0);
         _currentCharacterModel = playerModels[selectedOption].gameObject;
         SetCharacterModel(_currentCharacterModel);
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

        _playerVel = Vector3.zero;
    }
    public void SetCharacterModel(GameObject characterModel)
    {
        _currentCharacterModel = characterModel;
        playerModels[selectedOption].gameObject.SetActive(true);
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
            if(interactable is Item item && objectInHand == null)
            {
                _animator.SetTrigger("activate");
                ValidateEquippedItem(item.Data);
                Destroy(item.gameObject);
            }
            else
            {
                _animator.SetTrigger("activate");
                interactable.Interact();
            }

          
        }
    }
    private void Use()
    {
        if(objectInHand != null && objectInHand is Item item)
        {
            _animator.SetTrigger("UseItem");
            item.Use();
        }
    }
    private void Throw()
    {
        if (objectInHand != null && objectInHand is Item item)
        {
            _animator.SetTrigger("ThrowItem");
        }
    }
    //Called by the throw animation to detach the item from the players hand and reset the item state. 
    public void ThrowEvent()
    {
        if (objectInHand != null && objectInHand is Item item)
        {
            item.HandleDeactivateItemState();

            item.transform.SetParent(null);

         
         
            item.transform.position = handPos.position + transform.forward * 1f + Vector3.up * 1f;

       
            item.ItemPulse(transform.forward);
            objectInHand = null;
        }   
    }
    private void Drop()
    {
        if (objectInHand != null && objectInHand is Item item)
        {
            _animator.SetTrigger("DropItem");
            if (item.OverrideController != null)
            {
                RemoveOverrideController(item);
            }
            item.HandleDeactivateItemState();
            item.transform.SetParent(null);
            objectInHand = null;
        }
    }
    private void ValidateEquippedItem(BaseItemData data)
    {
        if(data == null) return;
        if(data.Prefab != null && data.Prefab is Item item)
        {
            Item instance = Instantiate(item);
            AddObjectToRightHand(instance);
        }
    }
    private void AddObjectToRightHand(IInteractable obj)
    {
        if (obj == null) return;
    
            //when an object is in the hand if its a condition object it will be thrown if not it will attempt to be readded to the player inventory
            if(objectInHand != null)
            {
            if (objectInHand is Item item)
            {
                if (item.OverrideController != null)
                {
                    RemoveOverrideController(item);
                }
                item.HandleDeactivateItemState();
                objectInHand.Interact();
                objectInHand = null;
            }
        }
        //now that their is no more item in the hand we will add the object passed in to the hand

        //If the object in hand is of type Item we will see if the item has an override controller if it doess then our current controller
        //should be replaced with the override controller
        if (obj is Item equippedItem)
        {
            //override controller add here
            if (equippedItem.OverrideController != null)
            {
                SetAnimationOverride(equippedItem);
            }
            equippedItem.HandleActivateItemState();
            equippedItem.transform.SetParent(handPos);
            Quaternion rotationOffset = Quaternion.Inverse(equippedItem.HandlePoint.rotation) * equippedItem.transform.rotation;
            equippedItem.transform.rotation = handPos.rotation*rotationOffset;
            Vector3 offset = equippedItem.HandlePoint.position - equippedItem.transform.position; 
            equippedItem.transform.position = handPos.position - offset;
            objectInHand = equippedItem;
            playerLookAtTarget = null;
        }
    }
    private void RemoveOverrideController(Item item)
    {
        _animator.runtimeAnimatorController = originalController;
    }
    private void SetAnimationOverride(Item item)
    {
        _animator.runtimeAnimatorController = item.OverrideController;
    }
    public void ItemColliderToggle()
    {
        if (objectInHand is Item item)
        {
            if (item.BodyCol.enabled)
                item.BodyCol.enabled = false;
            else
                item.BodyCol.enabled = true;
        }
    }
    public void UpdatePlayerCharacter(int index)
    {
        playerModels[index].enabled = true;
    }
}