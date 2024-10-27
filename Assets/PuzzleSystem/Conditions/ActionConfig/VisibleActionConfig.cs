using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "VisibleActionConfig", menuName = "PuzzleSystem/ActionConfigs/VisibleActionConfig")]

public class VisibleActionConfig : ActionConfig
{
    public override void RunAction(Condition condition)
    {
        condition.GetComponent<MeshRenderer>().enabled = true;
    }

}
