using UnityEngine;
/// <summary>
/// A general condition for objects that need to be moved a specific distance in order for a condition to be met.
/// </summary>
[CreateAssetMenu(fileName = "MovementDistanceConfig", menuName = "PuzzleSystem/ConditionConfig/MovementDistance")]
public class MovementConditionConfig : ConditionConfig
{
    [SerializeField] float expectedDistance = 5;
    private Vector3 initialPosition;
    public override void EnterSetup(Condition conditionObject)
    {
        initialPosition = conditionObject.transform.position;      
    }
    public override void ConditionStatus(Condition conditionObject)
    {
        if (Vector3.Distance(initialPosition, conditionObject.transform.position) >= expectedDistance)
            ConfigConditionMet?.Invoke();
        
    }    
}
