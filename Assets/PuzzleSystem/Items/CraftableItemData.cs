using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents the data for a craftable item including the required components for making said item. 
/// </summary>
[CreateAssetMenu(fileName = "CraftableItem", menuName = "PuzzleSystem/ItemData/Craftable/CraftableItem")]
public  class CraftableItemData : BaseItemData
{
    [SerializeField]  List<CraftableComponentData> craftableComponents = new List<CraftableComponentData>();
    public List<CraftableComponentData> CraftableComponents => craftableComponents;

 
}
