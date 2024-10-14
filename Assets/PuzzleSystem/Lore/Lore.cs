using System;
using TMPro;
using UnityEngine;
/// <summary>
/// Represents the interactable lore around the map. It is case dependent so that every case has a unique lore to it. 
/// </summary>
public class Lore : MonoBehaviour,IInteractable, ICustomizableComponent
{
    [SerializeField, Tooltip("Place a title for the lore here")] string titleText;
    [SerializeField, Tooltip("Place a description explaining the lore here")] Description description;
    [SerializeField, Tooltip("represents icon image")] protected Sprite icon;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI body;
    private Guid id = new Guid();
    public Guid ID => id;
    void Awake()
    {
        title.text = titleText;
        body.text = description.Text;
    }
    public void Interact()
    {
        DisplayText();
    }
    public void DisplayText()
    {
        canvas.gameObject.SetActive(true);
    }
    public Payload GetPayload()
    {
        return new Payload { isEmpty = false, lore = this };
    }
}
