using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCatcher : MonoBehaviour
{
    public void ThrowEvent()
    {
        EventSheet.ThrowAnimationEvent?.Invoke();
    }
    public void ItemColliderToggle()
    {
        EventSheet.ItemColliderAnimationEvent?.Invoke();
    }
}
