using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceBlockConditionConfig", menuName = "PuzzleSystem/ConditionConfig/IceBlockConditionConfig")]
public class IceBlockConditionConfig : ConditionConfig
{
    public override void ConditionStatus(Condition conditionObject)
    {
        if (IcePuzzleManager.Instance.PuzzleComplete)
        {
            ConfigConditionMet?.Invoke();
        }
    }

    public override void EnterSetup(Condition conditionObject)
    {

    }
}
