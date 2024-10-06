using System;
using UnityEngine;
/// <summary>
/// Represents an interface between the puzzle and the expected condition
/// for meeting one of many possible conditions the puzzle has to be considered complete..
/// </summary>
[RequireComponent(typeof(Collider))]
public class Condition : MonoBehaviour, IInteractable, ICustomizableComponent
{
    public Action ConditionStatus;
    public static Action<Transform> ConditionPickUp;
    [SerializeField] ConditionConfig config;
    [SerializeField] Collider col;
    [SerializeField] CraftableItemData requiredItem;
    [SerializeField] ConditionEndPoint goal;
    [SerializeField] bool isPickUp;
    [SerializeField] bool isConditionMet;
    public ConditionConfig Config => config;
    public bool IsConditionMet
    {
        get { return isConditionMet; }
        set
        {
            bool original = isConditionMet;
            isConditionMet = value;
            if (original != value)
                ConditionStatus.Invoke();
        }
    }
    public void Awake()
    {
        col ??= GetComponent<Collider>();
        col.isTrigger = true;
    }
    public void Start()
    {
        config.EnterSetup(this);
    }
    public void Update()
    {
        IsConditionMet = config.ConditionStatus(this);
    }
    public void OnTriggerEnter(Collider other)
    {
        config.TriggerEntered(this, other);
    }
    public void OnTriggerStay(Collider other)
    {
        config.TriggerStayed(this, other);
    }
    public void OnTriggerExit(Collider other)
    {
        config.TriggerExited(this, other);
    }
    public void Interact()
    {
        if (isPickUp)
            ConditionPickUp.Invoke(transform);
        if (config is InteractConditionConfig iConfig)
        iConfig.Interact(this);
    }
    public Payload GetPayload() {
        if (isPickUp)
            return new Payload { isEmpty = true };
        else
            return new Payload { isEmpty = true };
    }
}