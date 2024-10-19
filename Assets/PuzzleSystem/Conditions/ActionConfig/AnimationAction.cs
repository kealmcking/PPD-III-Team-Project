using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationAction", menuName = "PuzzleSystem/ActionConfigs/AnimationAction")]
public class AnimationAction : ActionConfig
{
    [SerializeField] string nameOfAnimationState = "ConditionState";
    [SerializeField] int animationLayerIndex = 0;
    [SerializeField] AnimatorOverrideController overrideController;
    public override void RunAction(Condition condition)
    {
        condition.TryGetComponent(out Animator anim);
        if (anim != null && nameOfAnimationState== "ConditionState")
        {
            anim.runtimeAnimatorController = overrideController;
            anim.Play(nameOfAnimationState,animationLayerIndex);
        }
    }
}
