using UnityEngine;
[CreateAssetMenu(fileName = "MovementDistanceConfig", menuName = "PuzzleSystem/ConditionConfig/MovementDistance")]
public class MovementConditionConfig : ConditionConfig
{
    [SerializeField] float expectedDistance = 5;
    private Vector3 initialPosition;
    public override void EnterSetup(Condition conditionObject)
    {
        initialPosition = conditionObject.transform.position;
    }
    public override bool ConditionStatus(Condition conditionObject)
    {
        if (Vector3.Distance(initialPosition, conditionObject.transform.position) >= expectedDistance) return true;
        else return false;
    }    
}
