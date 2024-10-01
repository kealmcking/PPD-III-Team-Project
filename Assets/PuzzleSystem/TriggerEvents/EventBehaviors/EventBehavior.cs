using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBehavior : ScriptableObject
{
    
    public abstract void Trigger(Context context);
}
