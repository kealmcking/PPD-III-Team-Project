using System;
using UnityEngine;
/// <summary>
/// Represents the abstract base class of all condition configurations
/// Used to determine what needss to be done for the associated condition to be completed.
/// </summary>
public abstract class ConditionConfig : ScriptableObject, ICustomizableComponent
{
    public Action ConfigConditionMet;
    public abstract void EnterSetup(Condition conditionObject);
    public abstract void ConditionStatus(Condition conditionObject);
    public virtual void TriggerEntered(Condition conditionObject, Collider other) { }
    public virtual void TriggerExited(Condition conditionObject, Collider other) { }
    public virtual void TriggerStayed(Condition conditionObject, Collider other) { }
   
}
