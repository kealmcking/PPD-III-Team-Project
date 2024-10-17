using UnityEngine;

public abstract class ConditionEndpointConfig:ScriptableObject
{
    public abstract void RunConfiguration(ConditionEndPoint point);
}