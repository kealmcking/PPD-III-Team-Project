using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClue : BaseItem, IPickupable
{
    [SerializeField] Description clueDescription;
    public abstract void PickUp();
    
}
