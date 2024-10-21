using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FXColorChangeAction", menuName = "PuzzleSystem/ActionConfigs/FXColorChangeAction")]
public class FXColorChangeAction : ActionConfig
{
    
    [SerializeField] Color color;

    public override void RunAction(Condition condition)
    {
        ParticleSystem.MainModule module = condition.GetComponentInChildren<ParticleSystem>().main;
        module.startColor = color;
    }


}
