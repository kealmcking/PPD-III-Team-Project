using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        
        [Header("Dialogue/Dialogue Trees")]
        [SerializeField] private DialogueTree currentTree;
        [SerializeField] private Dialogue currentDialogueLine;
        [SerializeField] private int currentDialogueIndex = 0;

        [Header("UI Elements")]
        [SerializeField] private DialogueButton[] dialogueButtons;
        [SerializeField] private GameObject speakerTextContainer;
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private Image speakerImage;
        public DialogueButton lastClickedDialogueButton;

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
            initializeVar();
        }

        private void initializeVar()
        {
            currentDialogueIndex = 0;
            speakerText.text = "";
            speakerTextContainer.SetActive(false);
            speakerImage.sprite = currentTree.speaker.characterSprite_base;
        }
        
        // We can input the current dialogue tree here
        public void selectDialogueTree(DialogueTree newTree)
        {
            buttonInitialization();
        }

        // We can send the currently selected dialogue option here
        public void selectDialogueLine(Dialogue newLine)
        {
            currentDialogueLine = newLine;
            readDialogueLine();
        }
        
        // We can read said dialogue option
        public void readDialogueLine(int dialogueIndex = 0) {
            switchDialogueButtons(false);
            speakerTextContainer.SetActive(true);
            speakerText.text = currentTree.speaker.name + ": " + currentDialogueLine.response[dialogueIndex];
        }

        public void advanceToNextDialogueLine()
        {
            if (currentDialogueIndex < currentDialogueLine.response.Length - 1)
            {
                currentDialogueIndex++;
                readDialogueLine(currentDialogueIndex);
            }
            else { exitSpeakerDialogue(); }
        }

        public void exitSpeakerDialogue()
        {
            switchDialogueButtons(true);
            initializeVar();
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
