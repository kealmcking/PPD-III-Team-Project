using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
