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
        Vector3 direction = camera.transform.position - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
