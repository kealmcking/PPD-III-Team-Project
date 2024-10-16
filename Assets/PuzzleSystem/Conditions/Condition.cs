using System;
using UnityEngine;
/// <summary>
/// Represents an interface between the puzzle and the expected condition
/// for meeting one of many possible conditions the puzzle has to be considered complete..
/// </summary>
[RequireComponent(typeof(SphereCollider), typeof(EnableInteractUI))]
public class Condition : MonoBehaviour, IInteractable, ICustomizableComponent
{
    private Guid id = new Guid();
    public Guid ID => id;
    public Action ConditionStatus;
    [SerializeField] ConditionConfig config;
    [SerializeField] Collider col;
    [SerializeField] CraftableItemData requiredItem;
    [SerializeField] ConditionEndPoint goal;
    [SerializeField] EnableInteractUI interactUI;
    [SerializeField] bool isPickUp;
    [SerializeField] bool isConditionMet;
    [SerializeField] private bool hasBeenPickedUp = false;
    public ConditionConfig Config => config;
    public ConditionEndPoint Goal => goal;
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
        interactUI ??= GetComponent<EnableInteractUI>();
        col ??= GetComponent<SphereCollider>();
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
         if (isPickUp && !hasBeenPickedUp)
        {
            interactUI.ToggleCanvas();
          //  hasBeenPickedUp = true;
        }
           
        // if (config is InteractConditionConfig iConfig)
        // iConfig.Interact(this);
    }
    public GameObject GetObject()
    {
        return gameObject;
    }

    public bool CanPickup()
    {
        return isPickUp;
    }

    public bool HasBeenPickedUp()
    {
        return hasBeenPickedUp;
    }

    public void SetHasBeenPickedUp(bool set)
    {
        hasBeenPickedUp = set;
    }
}
