using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//make editor script for a button when make puzzle is pressed a window will pop-up that asks which case/motive you want to add it to then press ok.
//This will create a prefab and add the prefab to the correct scriptable object for this puzzle
public class Puzzle : MonoBehaviour
{
    //Puzzle complete event
    [SerializeField] List<Condition> conditions = new List<Condition>();
    [SerializeField] BaseClue reward;
    [SerializeField] AudioClip clip;
    [SerializeField] ParticleSystem vfx;
    [SerializeField] Transform rewardSpawnPosition;
    public bool IsComplete{get; private set;}
    public void OnEnable()
    {
        foreach (var condition in conditions)
        {
            condition.ConditionStatus += UpdateCondition;
        }
    }
    public void OnDisable()
    {
        foreach (var condition in conditions)
        {
            condition.ConditionStatus -= UpdateCondition;
        }
    }
    public void UpdateCondition()
    {
        foreach (var condition in conditions)
        {
            if (condition.IsConditionMet) continue;
            else return;
        }
        
        //Fire event to Director notifying puzzle complete
        //Fire off events for various updates when puzzle complete
        //sfx/vfx/etc
        if(vfx!=null)
        vfx.Play();
        if (clip != null) { }
          //playclip here  clip.
        Instantiate(reward).GameObject().transform.position = rewardSpawnPosition.position;
    }
}
