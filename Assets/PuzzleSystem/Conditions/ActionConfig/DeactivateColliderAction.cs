using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DeactivateColliderAction", menuName = "PuzzleSystem/ActionConfigs/DeactivateColliderAction")]
public class DeactivateColliderAction : ActionConfig
{
    public override void RunAction(Condition condition)
    {
       condition.BodyCol.enabled = false;
    }
}
