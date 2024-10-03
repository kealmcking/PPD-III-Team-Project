using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractConditionConfig : ConditionConfig
{
    public abstract void Interact(Condition conditionObject);
}
