using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invItem : MonoBehaviour
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
}
