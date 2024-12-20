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
   
    [SerializeField] bool isConditionMet = false;
    [SerializeField,Tooltip("When this condition is met do you want to set this conditions active state to false?")] bool setObjectFalseOnComplete = false;
    [SerializeField] Rigidbody rb;
    public bool IsInteractable { get; set; } = false;
 
    [SerializeField] bool isGate = false;
    public bool IsGate => isGate;
    [SerializeField] EnableInteractUI interactUI;
    Material denyMaterial;
    Material childDenyMaterial;
    public Material DenyMaterial => denyMaterial;  
    public Material ChildDenyMaterial => childDenyMaterial; 
    [SerializeField] AudioClip denyAudioClip;
    public AudioClip DenyAudioClip => denyAudioClip;
    [SerializeField] GameObject childToUpdate;
    public GameObject ChildToUpdate => childToUpdate;
    [SerializeField, Tooltip("List of all the craftable components required to create the items to finish this condition")] List<CraftableComponentData> components = new List<CraftableComponentData>(3);
    [SerializeField, Tooltip("create an empty transform and make it a child of the condition inside this field add the transform." +
        "The transforms will be the potential spawning points for the components used to craft the item needed to complete the condition." +
        "It is recommended that their is more positions than components to allow for more unpredictable possible spawn points for the components.")]
    List<Transform> componentPositions = new List<Transform>();
    [SerializeField] bool canConditionBeUsed = true;
    public Rigidbody RB => rb;
    public bool IsConditionMet => isConditionMet;
    public bool SetObjectFalseOnComplete => setObjectFalseOnComplete;

    public bool isInteractedWith = false;
    public Collider BodyCol => bodyCol;
    public void Awake()
    {
  
        interactCol ??= GetComponent<SphereCollider>();
        rb??= GetComponent<Rigidbody>();
        if(isGate)
        {
                foreach (var mat in GetComponent<Renderer>().materials)
                {
                    if (mat.name.Contains("OutlineTest"))
                    {
                        denyMaterial = mat;
                        break;
                    }
                
                }

            if (ChildToUpdate != null)
            {
               
                foreach (var mat in ChildToUpdate.GetComponent<Renderer>().materials)
                {

                    if (mat.name.Contains("OutlineTest"))
                    {
                        childDenyMaterial = mat;
                        break;
                    }
                }
            }

        }
   
        
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
        if (IsInteractable)
        {
            interactUI ??= GetComponent<EnableInteractUI>();
        }
    }
    public void Update()
    {
        if (config != null && canConditionBeUsed == true)
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
            interactUI.ToggleCanvasOff(true);
            ConditionStatus?.Invoke();
        }     
    }
    public void OnTriggerEnter(Collider other)
    {
        if (config != null && canConditionBeUsed == true)
            config.TriggerEntered(this, other);
    }  
    public void OnTriggerStay(Collider other)
    {
        if (config != null && canConditionBeUsed == true)
            config.TriggerStayed(this, other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (config != null && canConditionBeUsed == true)
            config.TriggerExited(this, other);
    }
    public void Interact()
    {
        if(IsInteractable && canConditionBeUsed == true)
        {
            interactUI.ToggleCanvasOff(true);
            isInteractedWith = true;
        }
       
    }
    public GameObject GetObject()
    {
        return gameObject;
    }
    public void BlockCondition(bool value)
    {
        canConditionBeUsed = value;
    }
}
