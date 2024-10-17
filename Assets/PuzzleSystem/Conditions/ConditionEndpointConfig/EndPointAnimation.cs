using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(fileName = "EndPointAnimation", menuName = "PuzzleSystem/ConditionEndpointConfig/EndPointAnimation")]
public class EndPointAnimation : ConditionEndpointConfig
{
    [SerializeField] string nameOfAnimationState;
    [SerializeField] int animationLayerIndex = 0;
    public override void RunConfiguration(ConditionEndPoint condition)
    {
        condition.TryGetComponent(out Animator anim);
        if (anim != null && nameOfAnimationState!= "")
        {
            anim.Play(nameOfAnimationState,animationLayerIndex);
        }
    }
}
