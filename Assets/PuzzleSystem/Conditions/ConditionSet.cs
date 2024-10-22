using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// represents a container to store conditions in. 
/// </summary>
public class ConditionSet : MonoBehaviour
{
    public Action ConditionSetComplete;
    [SerializeField] List<Condition> conditions = new List<Condition>();
    public List<Condition> Conditions => conditions;
    [SerializeField] bool deactivateSetAfterCompletion = true;
 
    public bool IsSetComplete { get; private set; }
    public bool DeactivateSetAfterCompletion => deactivateSetAfterCompletion;

    public void OnEnable()
    {
        foreach (var condition in conditions)
        {
            condition.ConditionStatus += UpdateConditionSet;
        }
    }
    public void OnDisable()
    {
        foreach (var condition in conditions)
        {
            condition.ConditionStatus -= UpdateConditionSet;
        }
    }

    public void UpdateConditionSet()
    {
        if (conditions.Count > 0)
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsConditionMet) return;
                if(condition.IsConditionMet && condition.SetObjectFalseOnComplete == true)
                {
                    condition.gameObject.SetActive(false);
                }
            }
            IsSetComplete = true;
            ConditionSetComplete?.Invoke();
        }
    }
}
