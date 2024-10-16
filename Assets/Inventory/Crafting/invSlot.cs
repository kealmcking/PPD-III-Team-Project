using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invSlot : MonoBehaviour
{
    public invItem curItem;
    [SerializeField] SlotValidationType type = SlotValidationType.None;
    public SlotValidationType Type => type;
    public void SetType(SlotValidationType type)
    {
        this.type = type;
    }
    public void setCurItem(invItem item)
    {
        curItem = item;
        item.transform.SetParent(transform);
        item.transform.position = transform.position;
    }
}
