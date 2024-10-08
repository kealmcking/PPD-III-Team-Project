using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogue;
    public bool hasBeenRead;
    public bool shouldCloseWindow;
    public string[] response;

    //public Clue requiredClue;
    public Dialogue newDialogue;

    public string animationString;
    public Lore unlockedLore;

    private string initialDialogue;
    private bool initialHasBeenRead;
    private bool initialShouldCloseWindow;
    private string[] initialResponse;
    private Dialogue initialNewDialogue;

    private void OnEnable()
    {
        initialDialogue = dialogue;
        initialHasBeenRead = hasBeenRead;
        initialShouldCloseWindow = shouldCloseWindow;
        initialResponse = (string[])response.Clone();
        initialNewDialogue = newDialogue;
    }

    public void ResetData()
    {
        dialogue = initialDialogue;
        hasBeenRead = initialHasBeenRead;
        shouldCloseWindow = initialShouldCloseWindow;
        response = (string[])initialResponse.Clone();
        newDialogue = initialNewDialogue;
    }
}
