using UnityEngine;

public abstract class ConditionConfig : ScriptableObject
{   
    public abstract bool ConditionStatus(Condition conditionObject);
}
