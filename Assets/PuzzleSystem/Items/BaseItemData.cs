using System;
using UnityEngine;
/// <summary>
/// Represents the abstract class for all data that is a potential physical/interactable item. 
/// </summary>
public abstract class BaseItemData : ScriptableObject, ICustomizableComponent
{

    [SerializeField] protected string itemName;
    [SerializeField] protected Description description;
    [SerializeField] protected Item itemPrefab;
    [SerializeField, Tooltip("represents the display icon")] protected Sprite icon;
    private Guid id = new Guid();
    public Guid ID => id;
    public Item Prefab => itemPrefab;
    public Sprite Icon => icon;
    public string Name => itemName;
    public Description Description => description;

   

}
