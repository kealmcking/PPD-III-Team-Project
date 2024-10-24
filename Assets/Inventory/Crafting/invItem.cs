using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class invItem : MonoBehaviour //IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] BaseItemData itemData;
    [SerializeField] SlotValidationType type = SlotValidationType.None;
    public SlotValidationType Type => type;
    public BaseItemData ItemData { get { return itemData; } private set { itemData = value; } }
    [SerializeField] Image iconImage;
    // Update is called once per frame
    public void SetType(SlotValidationType type)
    {
        this.type = type;
    }
    public void SetItemData(BaseItemData itemData)
    {
        ItemData = itemData;
        iconImage.sprite = itemData.Icon;
    }

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("got it " + itemData.Name);
        TooltipManager.instance.setAndShow(itemData.Name, itemData.Description.Text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("dont got it " + itemData.Name);
        TooltipManager.instance.hide();
    }*/
}
