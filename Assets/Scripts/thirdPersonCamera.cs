using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Input;

public class thirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform _thirdPersonCameraTarget;
    [SerializeField] float _sensitivity = 100f;
    [SerializeField] float _topClamp = 280f;
    [SerializeField] float _bottomClamp = 20f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    //makes sure all inputs have been processed to allow for smooth motion
    private void LateUpdate()
    {
        CameraInput();
    }
    void CameraInput()
    {
        float mouseY = InputManager.instance.getAimAmount().y * _sensitivity * Time.deltaTime;
        float mouseX = InputManager.instance.getAimAmount().x * _sensitivity * Time.deltaTime;         
        ApplyCameraRotation(-mouseY, mouseX);
        ApplyPlayerRotation();
    }
    private void ApplyCameraRotation(float pitch, float yaw)
    {
        _thirdPersonCameraTarget.rotation *= Quaternion.AngleAxis(yaw, Vector3.up);
        _thirdPersonCameraTarget.rotation *= Quaternion.AngleAxis(pitch, Vector3.right);
        
        Vector3 angles = _thirdPersonCameraTarget.localEulerAngles;
        angles.z = 0;
        if (angles.x > 180 && angles.x < _topClamp)
        {
            angles.x = _topClamp;
        }
        else if (angles.x < 180 && angles.x > _bottomClamp)
        {
            angles.x = _bottomClamp;
        }
        _thirdPersonCameraTarget.localEulerAngles = angles;
    }
    private void ApplyPlayerRotation()
    {
        float moveX = InputManager.instance.getMoveAmount().x;
        float moveY = InputManager.instance.getMoveAmount().y;
        //only rotate camera when player begins to move
        if (moveX != 0 || moveY != 0) 
        {
            transform.rotation = Quaternion.Euler(0, _thirdPersonCameraTarget.rotation.eulerAngles.y, 0);
            //reset y axis to face the same way as the player
            _thirdPersonCameraTarget.localEulerAngles = new Vector3(_thirdPersonCameraTarget.localEulerAngles.x, 0, 0);
        }
    }
}
