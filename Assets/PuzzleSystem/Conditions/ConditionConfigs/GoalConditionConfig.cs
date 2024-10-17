using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GoalConditionConfig", menuName = "PuzzleSystem/ConditionConfig/GoalConditionConfig")]
public class GoalConditionConfig : ConditionConfig
{
    
    public override void ConditionStatus(Condition conditionObject)
    { 
    }

    public override void EnterSetup(Condition conditionObject)
    {
        
    }

    public override void TriggerEntered(Condition conditionObject, Collider other)
    {
        other.TryGetComponent(out ConditionEndPoint end);
        if (end != null)
        {
            Debug.Log($"Collider triggered by: {other.gameObject.name}");
            ConfigConditionMet?.Invoke();
        }
            
    }
 
}
