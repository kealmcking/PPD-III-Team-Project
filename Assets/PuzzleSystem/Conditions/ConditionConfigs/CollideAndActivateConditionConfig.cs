using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollideAndActivateConfig", menuName = "PuzzleSystem/ConditionConfig/CollideAndActivateConfig")]
public class CollideAndActivateConditionConfig : ConditionConfig
{
    [SerializeField] CraftableItemData triggerCheck;
    bool canBeInteractedWith = false;

    public override void EnterSetup(Condition conditionObject)
    {

    }
    public override void ConditionStatus(Condition conditionObject)
    {
        if (conditionObject != null&& canBeInteractedWith == true && conditionObject.isInteractedWith)
            ConfigConditionMet?.Invoke();
    }

    

    public override void TriggerEntered(Condition conditionObject, Collider other)
    {
        other.TryGetComponent(out Item item);
        if (item != null)
        {
            if (item.Data.Name == triggerCheck.Name)
                canBeInteractedWith = true;
        }
    }
    public override void TriggerExited(Condition conditionObject, Collider other)
    {
        other.TryGetComponent(out Item item);
        if (item != null)
        {
            if (item.Data.Name == triggerCheck.Name)
                canBeInteractedWith = false;
        }
    }
}
