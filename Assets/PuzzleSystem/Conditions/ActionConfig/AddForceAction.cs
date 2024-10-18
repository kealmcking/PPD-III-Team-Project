using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceAction : ActionConfig
{
    [SerializeField] float forceMultiplier;
    [SerializeField] Vector3 forceDirectionOffset;
    public override void RunAction(Condition condition)
    {
        Vector3 forceDirection = condition.transform.TransformDirection(Vector3.forward + forceDirectionOffset);
        condition.RB.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
    }
   
}
