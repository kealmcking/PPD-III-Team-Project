using UnityEngine;
/// <summary>
/// Represents the motive of the killer. used for UI and for checking against the chosen murder motive 
/// to determine if the players guess for the motive is correct.
/// </summary>
[CreateAssetMenu(fileName = "MurderMotive", menuName = "PuzzleSystem/Killer/MurderMotive")]
public class MurderMotive : ScriptableObject, ICustomizableComponent
{
    [SerializeField] string motiveName;
    [SerializeField] Sprite icon;
    [SerializeField] Description description;

    public string Name => motiveName;
    public Sprite Icon => icon;
    public Description Description => description;
}