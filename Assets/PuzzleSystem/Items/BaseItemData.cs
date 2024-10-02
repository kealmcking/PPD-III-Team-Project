using System;
using UnityEngine;

public abstract class BaseItemData : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Description description;
    [SerializeField] protected Item itemPrefab;
    [SerializeField, Tooltip("Only place either a texture or a sprite for the icon, not both!")] protected Sprite icon;
    public Item Prefab => itemPrefab;
    public Sprite Icon => icon;
    public string ItemName => itemName;
    public Description Description => description;
}
