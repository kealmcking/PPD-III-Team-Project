using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public LayerMask layerMask;

    [Range(0,3f)] public float distanceToGround;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        
        if (anim)
        {

            float speed = Mathf.Sqrt(anim.GetFloat("horiz") + anim.GetFloat("vert"));
            float walkingThreshold = 0.3f;
            float ikWeight = speed > walkingThreshold ? 0f : 1f;
            
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            if (speed <= walkingThreshold)
            {
                // Left Foot
                RaycastHit hit;
                Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

                if (Physics.Raycast(ray, out hit, distanceToGround + 2.5f, ~layerMask))
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += distanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation);
                }
                
                // Right Foot
                ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

                if (Physics.Raycast(ray, out hit, distanceToGround + 2.5f, ~layerMask))
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += distanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation);
                }
            }
        }
    }
}
