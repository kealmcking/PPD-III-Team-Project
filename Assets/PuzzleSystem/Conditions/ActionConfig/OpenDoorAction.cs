using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OpenDoorAction", menuName = "PuzzleSystem/ActionConfigs/OpenDoorAction")]
public class OpenDoorAction : ActionConfig
{
    public override void RunAction(Condition condition)
    {
        if(condition.TryGetComponent(out Animator anim))
        {
            anim.Play("Open", 0);
        }
    }
}