using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField,Tooltip("Only place either a texture or a sprite for the icon, not both!")] protected Texture2D textureIcon;
    [SerializeField, Tooltip("Only place either a texture or a sprite for the icon, not both!")] protected Sprite spriteIcon;

  
}
