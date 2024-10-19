using UnityEngine;

public abstract class ActionConfig:ScriptableObject
{
    public abstract void RunAction(Condition condition);
}