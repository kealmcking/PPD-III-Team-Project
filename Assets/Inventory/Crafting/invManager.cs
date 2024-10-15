using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Input;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class invManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] invSlot[] itemSlots = new invSlot[8];
    [SerializeField] invSlot[] componentSlots = new invSlot[8];
    [SerializeField] invSlot[] craftTableSlots = new invSlot[3];
    [SerializeField] invItem itemPrefab;
    [SerializeField] RectTransform mainCanvas;
    [SerializeField] RectTransform nestedRect;
    invItem draggedItem;
    invSlot lastItemSlot;
    invSlot clickedSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable() {
        EventSheet.SendItemToInventory += itemPickedUp;
    }
    void OnDisable() {
        EventSheet.SendItemToInventory -= itemPickedUp;
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
            invSlot clickedSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<invSlot>();
            
            if (clickedSlot != null)
            {
               
                draggedItem = clickedSlot.curItem;
                clickedSlot.curItem = null;
                lastItemSlot = clickedSlot;
                draggedItem.transform.SetParent(transform.root, true);
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (draggedItem != null && eventData.pointerCurrentRaycast.gameObject != null && eventData.button == PointerEventData.InputButton.Left)
        {
            clickedSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<invSlot>();
            if (clickedSlot == null || !SlotValidation(clickedSlot, draggedItem))
            {
                lastItemSlot.setCurItem(draggedItem);
                draggedItem = null;
            }
            else if (clickedSlot != null && clickedSlot.curItem == null)
            {
                clickedSlot.setCurItem(draggedItem);
                draggedItem = null;
            }
            else if (clickedSlot != null && clickedSlot.curItem != null && SlotValidation(lastItemSlot, clickedSlot.curItem))
            {
                lastItemSlot.setCurItem(clickedSlot.curItem);
                clickedSlot.setCurItem(draggedItem);
                draggedItem = null;

            }

        }
        else if (eventData.pointerCurrentRaycast.gameObject == null && GameManager.instance.CraftTableActive)
        {
            lastItemSlot.setCurItem(draggedItem);
            draggedItem = null;
        }
        else {
            lastItemSlot.setCurItem(draggedItem);
            DropItem();
        }
    }
    public void itemPickedUp(Item item)
    {
        invSlot emptySlot = null;
        if (item.Data is CraftableComponentData)
        {
            for (int i = 0; i < componentSlots.Length; i++)   //check each slot in the inventory to see if it can be placed in the inventory or not
            {
                invSlot slot = componentSlots[i].GetComponent<invSlot>();

                if (slot.curItem == null)
                {
                    emptySlot = componentSlots[i]; // set the slot that the new item will be placed in
                    break;
                }
            }
        }
        else if(item.Data is CraftableItemData)
        {
            for (int i = 0; i < itemSlots.Length; i++)   //check each slot in the inventory to see if it can be placed in the inventory or not
            {
                invSlot slot = itemSlots[i].GetComponent<invSlot>();

                if (slot.curItem == null)
                {
                    emptySlot = itemSlots[i]; // set the slot that the new item will be placed in
                    break;
                }
            }
        }

        if (emptySlot != null)
        {
            invItem newItem = Instantiate(itemPrefab);
            newItem.SetType(emptySlot.Type);
            newItem.SetItemData(item.Data);
            emptySlot.setCurItem(newItem); // set the current item of that slot to the newItem that was picked up          
            Destroy(item.gameObject); // destroy the item in the world
        }
    }
    public void DropItem()
    {
        Item itemToDrop = Instantiate(draggedItem.ItemData.Prefab);
        draggedItem = null;
        lastItemSlot.curItem.transform.SetParent(null);
        Transform playerTransform = GameObject.FindWithTag("Player").transform;
        Vector3 playerPos = playerTransform.position;
        Vector3 playerForward = playerTransform.forward;
        itemToDrop.transform.position = playerPos + playerForward * 1f + Vector3.up * 1f;
        itemToDrop.ItemPulse(playerForward);
        
       
        lastItemSlot.TryGetComponent(out Item item);
        if (item != null)
        {
            Destroy(item.gameObject);
        }
        Destroy(lastItemSlot.curItem.gameObject);
    }
    private bool SlotValidation(invSlot slot, invItem item) 
    {
        if(slot.Type == item.Type)
            return true;
        else return false;
    }
}
public enum SlotValidationType
{
    None,
    Item,
    Component,
}
