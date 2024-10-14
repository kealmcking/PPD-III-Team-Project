using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Input;

public class invManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject[] slots = new GameObject[8];
    [SerializeField] GameObject itemPrefab;


    GameObject draggedItem;
    GameObject lastItemSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (draggedItem != null)
        {
            draggedItem.transform.position = UnityEngine.Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedItem = eventData.pointerCurrentRaycast.gameObject;
            invSlot slot = clickedItem.GetComponent<invSlot>();

            if (slot != null)
            {
                draggedItem = slot.curItem;
                slot.curItem = null;
                lastItemSlot = clickedItem;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(draggedItem != null && eventData.pointerCurrentRaycast.gameObject != null && eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedItem = eventData.pointerCurrentRaycast.gameObject;
            invSlot slot = clickedItem.GetComponent<invSlot>();

            if (slot != null && slot.curItem == null)
            {
                slot.setCurItem(draggedItem);
                draggedItem = null;
            }
            else if(slot != null && slot.curItem != null){
                lastItemSlot.GetComponent<invSlot>().setCurItem(slot.curItem);
                slot.setCurItem(draggedItem);
                draggedItem = null;
            }
        }
    }

    public void itemPickedUp(GameObject item)
    {
        GameObject emptySlot = null;

        for(int i = 0; i < slots.Length; i++)   //check each slot in the inventory to see if it can be placed in the inventory or not
        {
            invSlot slot = slots[i].GetComponent<invSlot>();

            if(slot.curItem == null)
            {
                emptySlot = slots[i]; // set the slot that the new item will be placed in
                break;
            }
        }

        if (emptySlot != null)
        {
            GameObject newItem = Instantiate(itemPrefab); //create newItem
            newItem.GetComponent<invItem>().itemScriptableObject = item.GetComponent<itemPickup>().itemScriptableObject; //set the scriptable object of the item picked up to the one that will be sent to the inventory
            newItem.transform.SetParent(emptySlot.transform.parent.parent.GetChild(2)); //set the parent of the new item to be the slot that was chosen

            emptySlot.GetComponent<invSlot>().setCurItem(newItem); // set the current item of that slot to the newItem that was picked up

            Destroy(item); // destroy the item in the world
        }
    }
}
