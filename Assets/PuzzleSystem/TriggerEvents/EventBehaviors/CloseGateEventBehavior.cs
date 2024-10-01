using UnityEngine;
[CreateAssetMenu(fileName = "CloseGate", menuName = "EventTriggers/AnimationTriggers/CloseGateSlideVertically")]
public class CloseGateEventBehavior : EventBehavior
{
    public override void Trigger(Context context)
    {
        if (context.animator != null)
            context.animator.Play("LowerGate");
    }
}
