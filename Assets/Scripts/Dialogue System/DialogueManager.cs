using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;

        [Header("Dialogue/Dialogue Trees")] 
        [SerializeField] private NPC currentNPC;
        [SerializeField] private DialogueTree currentTree;
        [SerializeField] private Dialogue currentDialogueLine;
        [SerializeField] private int currentDialogueIndex = 0;

        [Header("UI Elements")] 
        [SerializeField] private GameObject dialogueContainer;
        [SerializeField] private DialogueButton[] dialogueButtons;
        [SerializeField] private GameObject speakerTextContainer;
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private Image speakerImage;
        public DialogueButton lastClickedDialogueButton;

        [Header("Other")] 
        [SerializeField] private float timeBetweenLetters;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Resets some components of the dialogue data upon new dialogue selection
        private void resetDialogueData()
        {
            currentDialogueIndex = 0;
            speakerText.text = "";
            toggleSpeakerUI(false);
            speakerImage.sprite = currentNPC.characterSprite_base;
        }

        // We can send the currently selected dialogue option here
        public void selectDialogueLine(Dialogue newLine)
        {
            currentDialogueLine = newLine;
            readDialogueLine();
        }
        
        // We can read said dialogue option
        private void readDialogueLine(int dialogueIndex = 0) {
            toggleDialogueButtons(false);
            toggleSpeakerUI(true);
            speakerText.text = currentNPC.name + ": " + currentDialogueLine.response[dialogueIndex];
            StartCoroutine(typeLine(currentDialogueLine.response[dialogueIndex]));
        }

        // Used by a button to advance to the next line of dialogue
        // or, if lines have run out, exit the dialogue
        public void advanceToNextDialogueLine()
        {
            if (currentDialogueIndex < currentDialogueLine.response.Length - 1)
            {
                currentDialogueIndex++;
                readDialogueLine(currentDialogueIndex);
            }
            else { exitSpeakerDialogue(); }
        }

        // Exits the current dialogue and returns to the dialogue menu
        private void exitSpeakerDialogue()
        {
            toggleDialogueButtons(true);
            resetDialogueData();
            lastClickedDialogueButton.GetComponentInChildren<TMP_Text>().color = Color.gray;
        }

        // Just a method for easily resetting the dialogue when necessary
        private void buttonInitialization()
        {
            for (int i = 0; i < dialogueButtons.Length; i++)
            {
                dialogueButtons[i].dialogue = currentTree.dialogues[i];
                dialogueButtons[i].text.text = dialogueButtons[i].dialogue.dialogue;
            }
            
            toggleDialogueButtons(true);
        }
        
        // For enabling/disabling the dialogue buttons when necessary
        private void toggleDialogueButtons(bool set)
        {
            foreach (DialogueButton button in dialogueButtons)
            {
                button.gameObject.SetActive(set);
            }
        }

        // For enabling/disabling the speaker ui element when necessary
        public void toggleSpeakerUI(bool set)
        {
            speakerTextContainer.SetActive(set);
        }
        

        // For enabling the overall dialogue UI
        public void enableDialogueUI(NPC npc, DialogueTree tree)
        {
            currentNPC = npc;
            currentTree = tree;
            speakerImage.sprite = currentNPC.characterSprite_base;
            dialogueContainer.SetActive(true);
            buttonInitialization();
        }

        // For disabling the overall dialogue UI
        public void disableDialogueUI()
        {
            dialogueContainer.SetActive(false);
            toggleDialogueButtons(false);
            resetDialogueData();
            currentTree = null;
        }

        IEnumerator typeLine(string line)
        {
            speakerText.text = currentNPC.name + ": ";

            foreach (char letter in line.ToCharArray())
            {
                speakerText.text += letter;
                yield return new WaitForSeconds(timeBetweenLetters);
            }
        }
    }
    
}
