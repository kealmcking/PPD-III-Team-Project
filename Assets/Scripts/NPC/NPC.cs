using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC")]
public class NPC : ScriptableObject
{
    [Header("Dialogue")] 
    public DialogueTree[] trees;
    
    [Header("Sprites")]
    public Sprite characterSprite_base;
    public Sprite characterSprite_happy;
    public Sprite characterSprite_upset;
    public Sprite characterSprite_mad;
    public Sprite characterSprite_shock;
}
