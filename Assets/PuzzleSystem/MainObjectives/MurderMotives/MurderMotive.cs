using UnityEngine;
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