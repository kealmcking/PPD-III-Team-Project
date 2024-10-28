using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ActivateConditionConfig", menuName = "PuzzleSystem/ConditionConfig/ActivateConditionConfig")]
public class ActivateConditionConfig : ConditionConfig
{
    public override void ConditionStatus(Condition conditionObject)
    {
        if (conditionObject != null && conditionObject.isInteractedWith)
        {
            conditionObject.IsInteractable = false;
            ConfigConditionMet?.Invoke();
        }
    }

    public override void EnterSetup(Condition conditionObject)
    {
        conditionObject.IsInteractable = true;
    }

}
