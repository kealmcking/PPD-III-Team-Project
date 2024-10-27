using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SimonsSaysConditionConfig", menuName = "PuzzleSystem/ConditionConfig/SimonsSaysConditionConfig")]
public class SimonSaysConditionConfig : ConditionConfig
{
    public override void ConditionStatus(Condition conditionObject)
    {
        if(SimonSaysPuzzleManager.Instance.PuzzleComplete)
        {
            ConfigConditionMet?.Invoke();
        }
    }

    public override void EnterSetup(Condition conditionObject)
    {
   
    }
}
