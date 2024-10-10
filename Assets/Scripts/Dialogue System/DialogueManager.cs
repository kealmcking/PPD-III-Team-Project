using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour, IInteractable
    {
        public static DialogueManager instance;

        [SerializeField] private List<Suspect> suspects = new List<Suspect>();
        private Suspect currentSuspect;
        private int currentDay;

        [Header("Dialogue/Dialogue Trees")] 
        [SerializeField] private NPC currentNPC;
        [SerializeField] private DialogueTree currentTree;
        [SerializeField] private Dialogue currentDialogueLine;
        [SerializeField] private int currentDialogueIndex = 0;

        [Header("UI Elements")] 
        [SerializeField] private GameObject dialogueContainer;
        [SerializeField] public DialogueButton[] dialogueButtons;
        [SerializeField] private GameObject speakerTextContainer;
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private Image speakerImage;
        public DialogueButton lastClickedDialogueButton;

        private bool isActive;
        private bool isInDialogue;

        public static Action<bool> DialogueMenuActive;

        private Coroutine typingCoroutine;

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

        public bool GetIsActive()
        {
            return isActive;
        }

        public bool GetIsInDialogue()
        {
            return isInDialogue;
        }
        
        // We can send the currently selected dialogue option here
        public void selectDialogueLine(Dialogue newLine)
        {
            currentDialogueLine = newLine;
            readDialogueLine();
            isInDialogue = true;
        }
        
        // We can read said dialogue option
        private void readDialogueLine(int dialogueIndex = 0) {
            toggleDialogueButtons(false);
            toggleSpeakerUI(true);

            if (currentDialogueLine.animationString != "")
            {
                SuspectBeingInteractedWith().GetComponentInChildren<Animator>().SetTrigger(currentDialogueLine.animationString);
            }

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            if (currentDialogueLine != null && currentDialogueLine.response.Length > dialogueIndex)
            {
                speakerText.text = currentNPC.name + ": " + currentDialogueLine.response[dialogueIndex];
                typingCoroutine = StartCoroutine(typeLine(currentDialogueLine.response[dialogueIndex]));
            }
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
            else
            {
                exitSpeakerDialogue();
            }
        }

        // Exits the current dialogue and returns to the dialogue menu
        private void exitSpeakerDialogue()
        {
            toggleDialogueButtons(true);
            
            if (currentDialogueLine.shouldCloseWindow)
            {
                disableDialogueUI();
                InputManager.instance.EnableCharacterInputs();
            }

            changeDialogueIfNeeded();
            buttonInitialization();
            
            resetDialogueData();
            currentTree = currentNPC.trees[currentDay];
            isInDialogue = false;
        }

        private void changeDialogueIfNeeded()
        {
            if (currentTree == null) return;
            if (currentTree.dialogues == null) return;
            if (currentTree.dialogues.FindIndex(dialogue => dialogue == currentDialogueLine) == -1) return;
            
            int index = currentTree.dialogues.FindIndex(dialogue => dialogue == currentDialogueLine);
            
            if (index != -1 && currentDialogueLine.response != null)
            {
                currentTree.dialogues[index] = currentDialogueLine.newDialogue ? currentDialogueLine.newDialogue : currentDialogueLine;
            }
        }

        // Just a method for easily resetting the dialogue when necessary
        private void buttonInitialization()
        {
            if (currentTree == null) return;
            for (int i = 0; i < dialogueButtons.Length; i++)
            {
                if (i < currentTree.dialogues.Count)
                {
                    dialogueButtons[i].dialogue = currentTree.dialogues[i];
                    dialogueButtons[i].text.text = dialogueButtons[i].dialogue.dialogue;

                    if (!dialogueButtons[i].dialogue.hasBeenRead && dialogueButtons[i].dialogue != null)
                    {
                        dialogueButtons[i].text.color = Color.black;
                    }
                } 
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
            
            if (!isActive && !isInDialogue)
            {
                EventSystem.current.firstSelectedGameObject = dialogueButtons[0].gameObject;
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
            
            isActive = true;
        }

        // For enabling/disabling the speaker ui element when necessary
        public void toggleSpeakerUI(bool set)
        {
            speakerTextContainer.SetActive(set);
        }
        

        // For enabling the overall dialogue UI
        public void enableDialogueUI(Suspect suspect)
        {
            currentSuspect = suspect;
            currentNPC = currentSuspect.Npc;
            currentTree = currentNPC.trees[currentDay];
            speakerImage.sprite = currentNPC.characterSprite_base;
            dialogueContainer.SetActive(true);
            DialogueMenuActive.Invoke(true);
            InputManager.instance.DisableCharacterInputs();
            InputManager.instance.isInMenu = true;
            
            
            resetDialogueData(); // Reset at the beginning
            buttonInitialization();
            isActive = true;
        }

        // For disabling the overall dialogue UI
        public void disableDialogueUI()
        {
            InputManager.instance.isInMenu = true;
            EventSystem.current.SetSelectedGameObject(null);
            dialogueContainer.SetActive(false);
            toggleDialogueButtons(false);
            resetDialogueData();
            currentTree = null;
            currentSuspect.IsBeingInteractedWith = false;
            currentSuspect = null;
            DialogueMenuActive.Invoke(false);
            InputManager.instance.EnableCharacterInputs();
            isActive = false;
            isInDialogue = false;
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
        
        public void Interact()
        { 
            // Get this information later
            // enableDialogueUI(currentNPC, currentNPC.trees[0]);
           InputManager.instance.DisableCharacterInputs();
        }

        public Payload GetPayload()
        {
            return new Payload { isEmpty = true };
        }

        public Suspect SuspectBeingInteractedWith()
        {
            foreach (Suspect suspect in suspects)
            {
                if (suspect.IsBeingInteractedWith)
                {
                    return suspect;
                }
            }

            return null;
        }

        private void OnEnable()
        {
            EventSheet.SendSuspects += AddSuspectsToList;
            EventSheet.TodaysDayIndexIsThis += SetDay;
        }

        private void OnDisable()
        {
            EventSheet.SendSuspects -= AddSuspectsToList;
            EventSheet.TodaysDayIndexIsThis -= SetDay;

            foreach (var suspect in suspects)
            {
                foreach (var tree in suspect.Npc.trees)
                {
                    foreach (var dialogue in tree.dialogues)
                    {
                        dialogue.ResetData();
                    }
                }
            }

            currentNPC = null;
            currentTree = null;
            currentDialogueLine = null;
            currentDialogueIndex = 0;
            dialogueContainer.SetActive(false);
        }

        private void AddSuspectsToList(List<Suspect> context)
        {
            suspects = context;
        }

        private void SetDay(int day)
        {
            currentDay = day;
        }

        public void AddSuspect(Suspect sus)
        {
            suspects.Add(sus);
        }
        
    }
    
}
