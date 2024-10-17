using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents an end point or goal for a puzzle. 
/// for example can be used as the goal the player needs to push an object to or
/// for them to place an object on.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ConditionEndPoint : MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] List<ConditionEndpointConfig> config = new List<ConditionEndpointConfig>();

    public List<ConditionEndpointConfig> Config => config;
    private void Awake()
    {
        col ??= GetComponent<Collider>();      
       
    }
    public void PlayConfig()
    {
        if(config != null && config.Count > 0)  
            config.ForEach(c => c.RunConfiguration(this));
    }
}
