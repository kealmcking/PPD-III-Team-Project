using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField] private NPC npc;
    [SerializeField] public DialogueTree currentTree;

    [SerializeField] private bool isKiller;
    [SerializeField] private bool isGhost;
    private void Start()
    {
        currentTree = npc.trees[0];
        isKiller = npc.isKiller;
        isGhost = npc.isGhost;
    }

    private void OnMouseDown()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        DialogueManager.instance.enableDialogueUI(npc, currentTree);    
    } 
}
