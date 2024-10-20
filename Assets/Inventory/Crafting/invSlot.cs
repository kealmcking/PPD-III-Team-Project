using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class invSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        if (item == null) return;
        curItem = item;
        item.transform.SetParent(transform);
        item.transform.position = transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(curItem != null)
        TooltipManager.instance.setAndShow(curItem.ItemData.Name, curItem.ItemData.Description.Text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(curItem != null)
        TooltipManager.instance.hide();
    }
}
