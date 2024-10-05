using UnityEngine;
/// <summary>
/// Represents the data for a craftable item including the required components for making said item. 
/// </summary>
[CreateAssetMenu(fileName = "CraftableItem", menuName = "PuzzleSystem/ItemData/Craftable/CraftableItem")]
public class CraftableItemData : BaseItemData
{
    [SerializeField] CraftableComponentData[] craftableComponents = new CraftableComponentData[3];
    public CraftableComponentData[] CraftableComponent => craftableComponents;
}
