using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MovementDistanceConfig", menuName = "PuzzleSystem/ConditionConfig/MovementDistance")]
public class EndPointConditionConfig : ConditionConfig
{
    private bool didTrigger;
    public override void EnterSetup(Condition conditionObject)
    {
        
    }
    public override bool ConditionStatus(Condition conditionObject)
    {
        return didTrigger;
    }
    public override void TriggerEntered(Condition conditionObject, Collider other)
    {     
        if(other.gameObject.GetComponent<ConditionEndPoint>() == conditionObject.Goal)
        {
            didTrigger = true;
        }
    }

}
