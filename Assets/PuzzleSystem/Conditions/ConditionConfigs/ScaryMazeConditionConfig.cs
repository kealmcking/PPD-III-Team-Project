using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScaryMazeConditionConfig", menuName = "PuzzleSystem/ConditionConfig/ScaryMazeConditionConfig")]
public class ScaryMazeConditionConfig : ConditionConfig
{
    public override void ConditionStatus(Condition conditionObject)
    {
        if (ScaryMazePuzzleManager.Instance.PuzzleComplete)
        {
            ConfigConditionMet?.Invoke();
        }
    }

    public override void EnterSetup(Condition conditionObject)
    {

    }
}
