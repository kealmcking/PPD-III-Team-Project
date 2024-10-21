using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
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
[ExecuteInEditMode]
public class Puzzle : MonoBehaviour, ICustomizableComponent
{
    [SerializeField] List<ConditionSet> conditionSets = new List<ConditionSet>();
    [SerializeField] AudioClip clip;
    [SerializeField] ParticleSystem vfx;
    private int setIndex = 0;
    [SerializeField, Tooltip("Simply the position where the reward(clue) will be spawned. after completing the puzzle.")] Transform rewardSpawnPosition;
    private Guid id = new Guid();
    [SerializeField] BaseClueData reward;
    public BaseClueData Reward { get { return reward; } set { reward = value; } }
    public Guid ID => id;
    public bool IsComplete { get; private set; } = false;
    AudioSource src;

    private void Awake()
    {
        conditionSets[0].gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        
        foreach (var set in conditionSets)
        {
                set.ConditionSetComplete += UpdatePuzzle;
        }
            
    }
    private void OnDisable()
    {
        foreach (var set in conditionSets)
        {
            set.ConditionSetComplete -= UpdatePuzzle;
        }
    }
    public void UpdatePuzzle()
    {
        if (conditionSets.Count > 0)
        {
           if(conditionSets.Count > setIndex)
           {
                if(conditionSets[setIndex].DeactivateSetAfterCompletion == true)
                conditionSets[setIndex].gameObject.SetActive(false);
                conditionSets[setIndex++].gameObject.SetActive(true);
            }
            //Fire off events for various updates when puzzle complete
            //sfx/vfx/etc
            if (vfx != null)
                vfx.Play();
            if (clip != null) { }
            //playclip here clip.
            if (Reward != null)
                Instantiate(Reward.Prefab).GameObject().transform.position = rewardSpawnPosition.position;

            IsComplete = true;

        }
    }


#if UNITY_EDITOR
    private void Reset()
    {
        AddConditionSet();
    }
       private void AddConditionSet()
    {
     
        if (transform.Find("ConditionSet") == null)
        {
            GameObject conditionSetObject = new GameObject("ConditionSet");
            conditionSetObject.transform.SetParent(transform);
            conditionSetObject.AddComponent<ConditionSet>(); 
        }
    }
#endif   
}
