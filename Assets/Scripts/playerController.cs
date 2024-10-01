using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController charController;
    [SerializeField] LayerMask ignoreMask;

    private Camera _mainCam;

    private float horizInput;
    private float vertInput;

    [Header("Player Stats - Movement Mods")]
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
        _walkSpeed = _origSpeed;
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDistance, Color.red);
        movement();
        crouch();
        sprint();
    }


    void movement()
{
        _moveDir = Input.GetAxis("Horizontal") * transform.right +
                      Input.GetAxis("Vertical") * transform.forward;
        charController.Move(_moveDir * _walkSpeed * Time.deltaTime);
    }

    void sprint()
{

    if (Input.GetButtonDown("Sprint") && !_isCrouching)
    {
        _walkSpeed *= _sprintMod;
        _isSprinting = true;
    }
    else if (Input.GetButtonUp("Sprint") && !_isCrouching)
    {
        _walkSpeed = _origSpeed;
        _isSprinting = false;
    }
}

    void crouch()
    {
        if (Input.GetButtonDown("Crouch") && !_isSprinting)
        {
            _walkSpeed *= _crouchMod;
            _newHeight = _crouchHeight;
            _isCrouching = true;
        }
        else if (Input.GetButtonUp("Crouch") && !_isSprinting)
        {
            _walkSpeed = _origSpeed;
            _newHeight = _origHeight;
            _isCrouching = false;
        }
        charController.height = Mathf.SmoothDamp(charController.height, _newHeight, ref _curVel, _crouchTime);
    }
}