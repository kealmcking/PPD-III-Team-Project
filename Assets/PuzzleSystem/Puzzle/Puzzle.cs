using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
//make editor script for a button when finalize puzzle is pressed a window will pop-up that asks which case/motive should be a list of all the Motive class scriptable objects select one you want to add it to then press ok.
//This will create a prefab of this puzzle and add the prefab to this folder 'Assets\PuzzleSystem\PrefabDump\Puzzles'. and then add the prefab to the selected Motives list of puzzles if no scriptable object motive is made then create one this newly created motive will be placed in Assets\PuzzleSystem\Motives\SO
//then here add the prefab to the newly created motives list of puzzles
/// <summary>
/// Represents the physical puzzle in the game. contains conditions which need to be met to win the puzzle. 
/// Additionally, it contains the reward(typically a clue), and the potential for components used to build 
/// the required item to complete the puzzle.
/// </summary>
/// 
[RequireComponent(typeof(AudioSource))]
public class Puzzle : MonoBehaviour, ICustomizableComponent
{
    [SerializeField] List<Condition> conditions = new List<Condition>();   
    [SerializeField] AudioClip clip;
    [SerializeField] ParticleSystem vfx;
    [SerializeField, Tooltip("List of all the craftable components required to create the items to finish this puzzle")] List<CraftableComponentData> components = new List<CraftableComponentData>(3);
    [SerializeField, Tooltip("create an empty transform and make it a child of the puzzle inside this field add the transform." +
        "The transforms will be the potential spawning points for the components used to craft the item needed to complete the puzzle." +
        "It is recommended that their is more positions than components to allow for more unpredictable possible spawn points for the components.")]
    List<Transform> componentPositions = new List<Transform>();
    
    [SerializeField, Tooltip("Simply the position where the reward(clue) will be spawned. after completing the puzzle.")] Transform rewardSpawnPosition;
    private Guid id = new Guid();
    public BaseClueData Reward { get; set; } = null;
    public Guid ID => id;
    public bool IsComplete { get; private set; } = false;
    AudioSource src;

    private void OnEnable()
    {
        conditions.ForEach(c => c.ConditionStatus += UpdateCondition);
        if (components.Count > 0 && componentPositions.Count > 0) 
        components.ForEach((c) => {Instantiate(c.Prefab).GameObject().transform.position = Randomizer.GetRandomizedObjectFromListAndRemove(ref componentPositions).position;});
    }
    private void OnDisable()
    {
        foreach (var condition in conditions)
        {
            condition.ConditionStatus -= UpdateCondition;
        }
    }
    public void UpdateCondition()
    {
        if (conditions.Count > 0)
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsConditionMet) return;
            }

            //Fire off events for various updates when puzzle complete
            //sfx/vfx/etc
            if (vfx != null)
                vfx.Play();
            if (clip != null) { }
            //playclip here clip.
            if (Reward != null)
                Instantiate(Reward.Prefab).GameObject().transform.position = rewardSpawnPosition.position;

            conditions.ForEach((c) => Destroy(c.gameObject));
            IsComplete = true;
        }
    }
}
