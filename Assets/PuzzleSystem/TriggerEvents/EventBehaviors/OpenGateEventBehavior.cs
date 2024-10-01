using UnityEngine;
[CreateAssetMenu(fileName = "OpenGate", menuName = "EventTriggers/AnimationTriggers/OpenGateSlideVertically")]
public class OpenGateEventBehavior : EventBehavior
{
    public override void Trigger(Context context)
    {
        if(context.animator != null)
        context.animator.Play("RaiseGate");
    }
}
