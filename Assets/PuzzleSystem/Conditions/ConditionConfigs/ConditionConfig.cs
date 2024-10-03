using UnityEngine;

public abstract class ConditionConfig : ScriptableObject, ICustomizableComponent
{
    public abstract void EnterSetup(Condition conditionObject);
    public abstract bool ConditionStatus(Condition conditionObject);
    public virtual void TriggerEntered(Condition conditionObject, Collider other) { }
    public virtual void TriggerExited(Condition conditionObject, Collider other) { }
    public virtual void TriggerStayed(Condition conditionObject, Collider other) { }
   
}
