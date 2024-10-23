using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchUIManager : MonoBehaviour
{
    [SerializeField] List<CraftableItemData> recipes = new List<CraftableItemData>();
    [SerializeField] invSlot[] tableSlots;
    [SerializeField] GameObject generateButton;
    [SerializeField] GameObject vfx;
    [SerializeField, Tooltip("set this to the crafting table")] AudioSource aud;
    [SerializeField] AudioClip craftingCompleteSound;

    [Header("Cogs")]
    [SerializeField] Animator topCogAnimator;
    [SerializeField] Animator leftCogAnimator;
    [SerializeField] Animator rightCogAnimator;
    string cogAnimatorState = "coggers";
    
    
    public Item matchedItem;
    public  void Awake()
    {
        generateButton.SetActive(false);
    }
    public void Update()
    {
        if (CheckSlotItemCount())
        {
            FindRecipe();
        }
    }
    private bool CheckSlotItemCount()
    {
        foreach (var slot in tableSlots)
        {
            if (slot.curItem == null)
            {
                matchedItem = null;
                return false;
            }
            
        }
        return true;
    }
    public void CreateItem()
    {
        StartCoroutine(cogSpinner());
        generateButton.SetActive(false);
    }

    IEnumerator cogSpinner()
    {
        //play audio / vfx
        topCogAnimator.enabled = true;
        leftCogAnimator.enabled = true;
        rightCogAnimator.enabled = true;
        yield return new WaitForSeconds(5);
        topCogAnimator.enabled = false;
        leftCogAnimator.enabled = false;
        rightCogAnimator.enabled = false;
        aud.PlayOneShot(craftingCompleteSound);
        Item itemToDrop = Instantiate(matchedItem.Data.Prefab);
        matchedItem = null;
        foreach (var slot in tableSlots)
        {
            Destroy(slot.curItem.gameObject);
            slot.curItem = null;
        }
        Transform benchTransform = GameObject.FindWithTag("CraftTable").transform;
        Vector3 benchPos = benchTransform.position;
        Vector3 benchForward = benchTransform.forward;
        itemToDrop.transform.position = benchPos + benchForward * 1f + Vector3.up * 1f;
        itemToDrop.ItemPulse(benchForward);

    }

    private void FindRecipe()
    {
        if (matchedItem != null) return;
        List<CraftableComponentData> materials = new List<CraftableComponentData>();
        foreach (var slot in tableSlots)
        {
            materials.Add(slot.curItem.ItemData as CraftableComponentData);
        }
        foreach (var recipe in recipes)
        {
            if(ValidateMatch(materials, recipe.CraftableComponents))
            {
                matchedItem = recipe.Prefab;
                generateButton.SetActive(true);
                return;
            }
        }
    }
    //change this to a utility function
    private bool ValidateMatch(List<CraftableComponentData> materials, List<CraftableComponentData> recipeComponents)
    {
        // Group materials and count occurrences
        var materialCounts = materials.GroupBy(m => m)
                                      .ToDictionary(g => g.Key, g => g.Count());

        // Group recipe components and count occurrences
        var recipeCounts = recipeComponents.GroupBy(r => r)
                                           .ToDictionary(g => g.Key, g => g.Count());

        // Check if both dictionaries have the same number of keys
        if (materialCounts.Count != recipeCounts.Count)
            return false;

        // Compare counts for each component
        foreach (var kvp in recipeCounts)
        {
            if (!materialCounts.TryGetValue(kvp.Key, out int materialCount))
                return false; // Component not found in materials

            if (materialCount != kvp.Value)
                return false; // Counts do not match
        }

        // All components match with correct counts
        return true;
    }

    public void Close()
    {
        Input.InputManager.instance.EnableCharacterInputs();
        GameManager.instance.DeactivateCraftTableUI();
        GameManager.instance.DeactivateInventoryUISecondary();
    }

    public void OnEnable()
    {
        topCogAnimator.Play(cogAnimatorState);
        leftCogAnimator.Play(cogAnimatorState);
        rightCogAnimator.Play(cogAnimatorState);
        topCogAnimator.enabled = false;
        leftCogAnimator.enabled = false;
        rightCogAnimator.enabled = false;
    }
}


