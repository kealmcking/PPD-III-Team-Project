using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUIElementTowardsPlayer : MonoBehaviour
{

    private GameObject camera;
    
    // Start is called before the first frame update
    void Awake()
    { 
        camera = Camera.main.gameObject;
    }
    
    void LateUpdate()
    {
        transform.LookAt(camera.transform);
    }
}
