using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemCollisionConditionConfig", menuName = "PuzzleSystem/ConditionConfig/ItemCollisionConditionConfig")]
public class ItemCollisionConditionConfig : ConditionConfig
{
    [SerializeField] CraftableItemData triggerCheck;
    public override void ConditionStatus(Condition conditionObject)
    {

    }

    public override void EnterSetup(Condition conditionObject)
    {

    }

    public override void TriggerEntered(Condition conditionObject, Collider other)
    {
        other.TryGetComponent(out Item item);
        if (item != null)
        {
            if(item.Data.Name == triggerCheck.Name)
            ConfigConditionMet?.Invoke();
        }

    }

}
