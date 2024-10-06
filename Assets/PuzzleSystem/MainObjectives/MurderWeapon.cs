using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents the weapon of the killer. used for UI and for checking against the chosen murder weapon
/// to determine if the players guess for the weapon is correct.
/// </summary>
[CreateAssetMenu(fileName = "MurderWeapon", menuName = "PuzzleSystem/Killer/MurderWeapon")]
public class MurderWeapon : ScriptableObject, ICustomizableComponent
{
    [SerializeField] string weaponName;
    [SerializeField] Sprite icon;
    [SerializeField] Description description;

    public string Name => weaponName;
    public Sprite Icon => icon;
    public Description Description => description;
}
