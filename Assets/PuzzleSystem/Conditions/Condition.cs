using System;
using UnityEngine;

public class Condition : MonoBehaviour
{

    public Action ConditionStatus;
    [SerializeField] ConditionConfig config;
 
    public bool IsConditionMet 
    {
        get 
        { 
            return IsConditionMet; 
        }        
        set 
        {  
            bool original = IsConditionMet;
            IsConditionMet = value;
            if (original != value)
                ConditionStatus.Invoke();
        } 
    }
    public void Update()
    {
        IsConditionMet = config.ConditionStatus(this);
    }
}
