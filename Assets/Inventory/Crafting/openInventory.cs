using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class openInventory : MonoBehaviour
{
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject topCog;
    [SerializeField] GameObject leftCog;
    [SerializeField] GameObject rightCog;

    private float lerpSpeed = 0.2f;

    private Quaternion targetRotation;

    public GameObject player;
    public playerController playerController;

    public bool isInventoryOpen;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<playerController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetButtonDown("Inventory"))
        {
            if (menuActive == null)
            {
                stateInventoryOpen();
                menuActive = menuInventory;
                menuInventory.SetActive(isInventoryOpen);
                rotateCogs();

            }
            else if (menuActive == menuInventory)
            {
                stateInventoryClose();
            }
        }
    }

    public void stateInventoryOpen()
    {
        isInventoryOpen = !isInventoryOpen;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateInventoryClose()
    {
        isInventoryOpen = !isInventoryOpen;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isInventoryOpen);
        menuActive = null;
    }

    public void rotateCogs()
    {
        topCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        leftCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        rightCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }
}
