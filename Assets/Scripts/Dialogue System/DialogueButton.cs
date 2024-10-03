using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public class DialogueButton : MonoBehaviour, IPointerClickHandler
    {
        public Dialogue dialogue;
        public TMP_Text text;

        public Color selectedColor;

        private void Awake()
        {
            dialogue.hasBeenRead = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            DialogueManager.instance.selectDialogueLine(dialogue);
            DialogueManager.instance.lastClickedDialogueButton = this;
            updateReadStatus();
        }

        private void updateReadStatus()
        {
            dialogue.hasBeenRead = true;
            text.color = selectedColor;
        }
    }
}
