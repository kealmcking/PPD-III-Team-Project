using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Represents an interface between the puzzle and the expected condition
/// for meeting one of many possible conditions the puzzle has to be considered complete..
/// </summary>
[RequireComponent(typeof(SphereCollider), typeof(EnableInteractUI), typeof(Rigidbody))]
[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Condition : MonoBehaviour, IInteractable, ICustomizableComponent
{
    private Guid id = new Guid();
    public Guid ID => id;
    public Action ConditionStatus;
    [SerializeField] ConditionConfig config;
    [SerializeField] Collider bodyCol;
    [SerializeField] SphereCollider interactCol;
    [SerializeField, Tooltip("Add any type of event that you want to occur on this Condition that is defined as a" +
    "Action Config. For instance to play an animation like if you want the Condition to slide over or open up add the AnimationActionConfig " +
    "Scriptable Object here You can stack multiple for multiple effects.")]
    List<ActionConfig> actions = new List<ActionConfig>();
    [SerializeField] EnableInteractUI interactUI;
  
    [SerializeField] bool isConditionMet = false;
    [SerializeField,Tooltip("When this condition is met do you want to set this conditions active state to false?")] bool setObjectFalseOnComplete = false;
    [SerializeField] Rigidbody rb;
    [SerializeField] bool isInteractable;
    [SerializeField, Tooltip("List of all the craftable components required to create the items to finish this condition")] List<CraftableComponentData> components = new List<CraftableComponentData>(3);
    [SerializeField, Tooltip("create an empty transform and make it a child of the condition inside this field add the transform." +
        "The transforms will be the potential spawning points for the components used to craft the item needed to complete the condition." +
        "It is recommended that their is more positions than components to allow for more unpredictable possible spawn points for the components.")]
    List<Transform> componentPositions = new List<Transform>();
    [SerializeField] bool canConditionNotBeUsed = true;
    public Rigidbody RB => rb;
    public bool IsConditionMet => isConditionMet;
    public bool SetObjectFalseOnComplete => setObjectFalseOnComplete;
    public void Awake()
    {
        if (isInteractable)
        {
            interactUI ??= GetComponent<EnableInteractUI>();
        }    
        interactCol ??= GetComponent<SphereCollider>();
        rb??= GetComponent<Rigidbody>();
       
        
    }
    private void OnEnable()
    {
        if (components.Count > 0 && componentPositions.Count > 0)
            components.ForEach((c) => { Instantiate(c.Prefab).GameObject().transform.position = Randomizer.GetRandomizedObjectFromListAndRemove(ref componentPositions).position; });
        config.ConfigConditionMet += SendStatusUpdate;
        EventSheet.GateConditionStatus += BlockCondition;
    }
    private void OnDisable()
    {
        config.ConfigConditionMet -= SendStatusUpdate;
        EventSheet.GateConditionStatus -= BlockCondition;
    }
    public void Start()
    {
        if (config != null)
            config.EnterSetup(this);
    }
    public void Update()
    {
        if (config != null && canConditionNotBeUsed == true)
            config.ConditionStatus(this);
    }
    public void SendStatusUpdate()
    {
        if (!isConditionMet)
        {
            if(actions!=null && actions.Count > 0)
            {
                actions.ForEach(a => a.RunAction(this));
            }

            isConditionMet = true;
            ConditionStatus?.Invoke();
        }     
    }
    public void OnTriggerEnter(Collider other)
    {
        if (config != null && canConditionNotBeUsed == true)
            config.TriggerEntered(this, other);
    }  
    public void OnTriggerStay(Collider other)
    {
        if (config != null && canConditionNotBeUsed == true)
            config.TriggerStayed(this, other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (config != null && canConditionNotBeUsed == true)
            config.TriggerExited(this, other);
    }
    public void Interact()
    {
        if(isInteractable && canConditionNotBeUsed == true)
        interactUI.ToggleCanvas();
    }
    public GameObject GetObject()
    {
        return gameObject;
    }
    public void BlockCondition(bool value)
    {
        canConditionNotBeUsed = value;
    }
}
