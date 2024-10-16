using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EndPointConfig", menuName = "PuzzleSystem/ConditionConfig/EndPointCondition")]
public class EndPointConditionConfig : ConditionConfig
{
    private bool didTrigger = false;
    public override void EnterSetup(Condition conditionObject)
    {
        
    }
    public override bool ConditionStatus(Condition conditionObject)
    {
       
        return false;
    }
   
}
