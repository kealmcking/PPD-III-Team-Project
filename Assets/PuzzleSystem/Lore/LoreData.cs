using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Lore", menuName = "PuzzleSystem/LoreData/Lore")]
public class LoreData : ScriptableObject
{
    [SerializeField, Tooltip("Place a title for the lore here")] string titleText;
    [SerializeField, Tooltip("Place a description explaining the lore here")] Description description;
    [SerializeField, Tooltip("represents icon image")] Sprite icon;

    public Sprite Icon => icon;
    public string Name => titleText;
    public Description Description => description;
}
