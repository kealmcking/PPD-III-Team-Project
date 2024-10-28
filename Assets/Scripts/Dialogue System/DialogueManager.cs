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
    [RequireComponent(typeof(AudioSource))]

    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        private Suspect currentSuspect;
        private int currentDay;
        [SerializeField] private AudioSource audioSource;

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
        public bool IsActive => isActive;
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

            //audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.Log("Dialogue audio Error");
            }
            else
            {
                audioSource.clip = audioManager.instance.dialogueMutter[UnityEngine.Random.Range(0,audioManager.instance.dialogueMutter.Length)];
                audioSource.volume = audioManager.instance.dialogueVol;
                audioSource.outputAudioMixerGroup = audioManager.instance.GetSFXAudioMixer();
                audioSource.loop = true;
                audioSource.playOnAwake = false;
            }
        }

        // Resets some components of the dialogue data upon new dialogue selection
        private void resetDialogueData()
        {
            currentDialogueIndex = 0;
            speakerText.text = "";
            toggleSpeakerUI(false);
            if (currentSuspect is Ghost ghost)
            {
                speakerImage.sprite = ghost.GhostData.Icon;
            }
            else
            {
                speakerImage.sprite = currentNPC.characterSprite_base;
            }
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
                currentSuspect.Anim.SetTrigger(currentDialogueLine.animationString);
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

            if (currentDialogueLine.unlockedMotive != null)
            {
                EventSheet.SendClueToTracker?.Invoke(currentDialogueLine.unlockedMotive.Prefab);
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
            if (!TutorialUIManager.Instance.DisplayDialogue)
            {
                TutorialUIManager.Instance.DisplayDialogueTutorial();
            }

            if (suspect is Ghost ghost)
            {
                currentSuspect = suspect;
                currentNPC = ghost.SuspectData.Npc;
                currentTree = currentNPC.trees[currentDay];
                speakerImage.sprite = ghost.GhostData.Icon;
                dialogueContainer.SetActive(true);
                DialogueMenuActive.Invoke(true);
                InputManager.instance.DisableCharacterInputs();           
                resetDialogueData(); // Reset at the beginning
                buttonInitialization();
                isActive = true;
            }
            else
            {
                currentSuspect = suspect;
                currentNPC = currentSuspect.Data.Npc;
                currentTree = currentNPC.trees[currentDay];
                speakerImage.sprite = currentNPC.characterSprite_base;
                dialogueContainer.SetActive(true);
                DialogueMenuActive.Invoke(true);
                InputManager.instance.DisableCharacterInputs();           
                resetDialogueData(); // Reset at the beginning
                buttonInitialization();
                isActive = true;
            }
            

        }

        // For disabling the overall dialogue UI
        public void disableDialogueUI()
        {
        
            EventSystem.current.SetSelectedGameObject(null);
            dialogueContainer.SetActive(false);
            toggleDialogueButtons(false);
            resetDialogueData();
            currentTree = null;
            currentSuspect = null;
            DialogueMenuActive.Invoke(false);
            InputManager.instance.EnableCharacterInputs();
            isActive = false;
            //isInDialogue = false;
        }

        IEnumerator typeLine(string line)
        {
            audioSource.Play();

            speakerText.text = currentNPC.name + ": ";
            bool insideTag = false;
            string currentText = "";

            foreach (char letter in line.ToCharArray())
            {
                if (letter == '<')
                {
                    insideTag = true;
                }

                if (insideTag)
                {
                    currentText += letter;
                    if (letter == '>')
                    {
                        insideTag = false;
                    }
                }
                else
                {
                    currentText += letter;
                    speakerText.text = currentText;
                    yield return new WaitForSeconds(timeBetweenLetters);
                }
            }

            speakerText.text = currentText;

            yield return new WaitForSeconds(2f);
            audioSource.Stop();
            
        }
        
   

        private void OnEnable()
        {
            EventSheet.TodaysDayIndexIsThis += SetDay;
        }

        private void OnDisable()
        {
         
            EventSheet.TodaysDayIndexIsThis -= SetDay;
            currentNPC = null;
            currentTree = null;
            currentDialogueLine = null;
            currentDialogueIndex = 0;
            dialogueContainer.SetActive(false);
        }

     
        private void SetDay(int day)
        {
            currentDay = day;
        }

       
        
    }
    
}
