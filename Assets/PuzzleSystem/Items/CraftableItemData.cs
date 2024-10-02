using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CraftableItem", menuName = "PuzzleSystem/Item/Craftable/CraftableItem")]
public class CraftableItemData : BaseItemData
{
    [SerializeField] CraftableComponentData[] craftableComponents = new CraftableComponentData[3];
    public CraftableComponentData[] CraftableComponent => craftableComponents;
}
