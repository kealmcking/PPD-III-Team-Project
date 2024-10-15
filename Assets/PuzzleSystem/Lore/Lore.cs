using System;
using TMPro;
using UnityEngine;
/// <summary>
/// Represents the interactable lore around the map. It is case dependent so that every case has a unique lore to it. 
/// </summary>
[RequireComponent(typeof(SphereCollider), typeof(EnableInteractUI))]
public class Lore : MonoBehaviour, IInteractable, ICustomizableComponent
{
    [SerializeField] LoreData data;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI body;
    [SerializeField] EnableInteractUI interactUI;
    [SerializeField] Collider col;

    private Guid id = new Guid();
    public Guid ID => id;
    void Awake()
    {
        col ??= GetComponent<SphereCollider>();
        col.isTrigger = true;
        interactUI ??= GetComponent<EnableInteractUI>();
        title.text = data.Name;
        body.text = data.Description.Text;
        canvas.gameObject.SetActive(false);
    }
    public void Interact()
    {
        DisplayText();
    }
    public void DisplayText()
    {
        if(!canvas.gameObject.activeSelf)
        canvas.gameObject.SetActive(true);
        else
            canvas.gameObject.SetActive(false);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
