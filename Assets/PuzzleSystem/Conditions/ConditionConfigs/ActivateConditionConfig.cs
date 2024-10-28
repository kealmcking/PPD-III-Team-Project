using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ActivateConditionConfig", menuName = "PuzzleSystem/ConditionConfig/ActivateConditionConfig")]
public class ActivateConditionConfig : ConditionConfig
{
    public override void ConditionStatus(Condition conditionObject)
    {
        if (conditionObject != null && conditionObject.isInteractedWith)
        ConfigConditionMet?.Invoke();
    }

    public override void EnterSetup(Condition conditionObject)
    {
        conditionObject.IsInteractable = true;
    }

    // Start is called before the first frame update
    
}
