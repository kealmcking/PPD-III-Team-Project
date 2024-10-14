using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invSlot : MonoBehaviour
{
    public GameObject curItem;
    
    public void setCurItem(GameObject item)
    {
        curItem = item;
        curItem.transform.position = transform.position;
    }
}
