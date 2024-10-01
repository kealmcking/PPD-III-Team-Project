using System;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        
        [Header("Dialogue/Dialogue Trees")]
        [SerializeField] private DialogueTree currentTree;
        [SerializeField] private Dialogue currentDialogueLine;
        [SerializeField] private int currentDialogueIndex;

        [Header("UI Elements")]
        [SerializeField] private DialogueButton[] dialogueButtons;
        [SerializeField] private GameObject lastClickedDialogueButton;
        [SerializeField] private GameObject speakerTextContainer;
        [SerializeField] private TMP_Text speakerText;

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
            
            // for debugging
            selectDialogueTree(currentTree);
        }
        
        // We can input the current dialogue tree here
        public void selectDialogueTree(DialogueTree newTree)
        {
            currentTree = newTree;
            buttonInitialization();
        }

        // We can send the currently selected dialogue option here
        public void selectDialogueLine(Dialogue newDialogue)
        {
            currentDialogueLine = newDialogue;
            readDialogueLine();
        }
        
        // We can read said dialogue option
        public void readDialogueLine(int dialogueIndex = 0) {
            switchDialogueButtons(false);
            speakerText.text = currentDialogueLine.response[dialogueIndex];
            speakerTextContainer.SetActive(true);
        }

        public void advanceToNextDialogueLine()
        {
            currentDialogueIndex++;
            readDialogueLine(currentDialogueIndex);
        }

        public void exitSpeakerDialogue()
        {
            switchDialogueButtons(true);
            speakerText.text = "";
            speakerTextContainer.SetActive(false);
            lastClickedDialogueButton.GetComponentInChildren<TMP_Text>().color = Color.gray;
        }

        private void switchDialogueButtons(bool set)
        {
            foreach (DialogueButton button in dialogueButtons)
            {
                button.gameObject.SetActive(set);
            }
        }
        
        public void buttonInitialization()
        {
            for (int i = 0; i < dialogueButtons.Length; i++)
            {
                dialogueButtons[i].dialogue = currentTree.dialogues[i];
                dialogueButtons[i].text.text = dialogueButtons[i].dialogue.dialogue;
            }
            
            switchDialogueButtons(true);
        }
    }
    
}
