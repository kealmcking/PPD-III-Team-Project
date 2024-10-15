using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClueData : ScriptableObject, ICustomizableComponent
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Description description;
    [SerializeField] protected Clue itemPrefab;
    [SerializeField, Tooltip("represents the display icon")] protected Sprite icon;
    private Guid id = new Guid();
    public Guid ID => id;
    public Clue Prefab => itemPrefab;
    public Sprite Icon => icon;
    public string Name => itemName;
    public Description Description => description;
}
