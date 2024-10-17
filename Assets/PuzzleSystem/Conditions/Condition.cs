using System;
using UnityEngine;
/// <summary>
/// Represents an interface between the puzzle and the expected condition
/// for meeting one of many possible conditions the puzzle has to be considered complete..
/// </summary>
[RequireComponent(typeof(Collider), typeof(EnableInteractUI))]
public class Condition : MonoBehaviour, IInteractable, ICustomizableComponent
{
    private Guid id = new Guid();
    public Guid ID => id;
    public Action ConditionStatus;
    [SerializeField] ConditionConfig config;
    [SerializeField] Collider col;
    [SerializeField, Tooltip("Represents a goal for the player to either reach or interact with to solve the Condition")] ConditionEndPoint endPoint;
    [SerializeField] EnableInteractUI interactUI;
    [SerializeField] bool isPickUp;
    [SerializeField] bool isConditionMet = false;
    [SerializeField] private bool hasBeenPickedUp = false;
    public ConditionConfig Config => config;
    public ConditionEndPoint Goal => endPoint;
    public bool IsConditionMet => isConditionMet;
    public void Awake()
    {
        interactUI ??= GetComponent<EnableInteractUI>();
        col ??= GetComponent<Collider>();

    }
    private void OnEnable()
    {
        config.ConfigConditionMet += SendStatusUpdate;
      
    }
    private void OnDisable()
    {
        config.ConfigConditionMet -= SendStatusUpdate;
    }
    public void Start()
    {
        if (config != null)
            config.EnterSetup(this);
    }
    public void Update()
    {
        config.ConditionStatus(this);
    }
    public void SendStatusUpdate()
    {
        if (!isConditionMet)
        {
            if(endPoint!=null)
            {
                endPoint.PlayConfig();
            }
            isConditionMet = true;
            ConditionStatus?.Invoke();
        }     
    }
    public void OnTriggerEnter(Collider other)
    {
        config.TriggerEntered(this, other);
    }  
    public void OnTriggerStay(Collider other)
    {
        if (config != null)
            config.TriggerStayed(this, other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (config != null)
            config.TriggerExited(this, other);
    }
    public void Interact()
    {
        interactUI.ToggleCanvas();
     /*   if (isPickUp && !hasBeenPickedUp)
        {
            interactUI.ToggleCanvas();
          //  hasBeenPickedUp = true;
        }*/
           
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
