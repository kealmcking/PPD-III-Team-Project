using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Dialogue dialogue;
        public TMP_Text text;
        public Image image;

        public Color selectedColor;
        public Color hoveredColor;
        public Color originalColor;

        private void Awake()
        {
            image = GetComponent<Image>();
            dialogue.hasBeenRead = false;
            selectedColor = Color.gray;
            originalColor = image.color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PressButton();
            Debug.Log("OnPointerClick");
        }

        public void PressButton()
        {
            DialogueManager.instance.selectDialogueLine(dialogue);
            DialogueManager.instance.lastClickedDialogueButton = this;
            updateReadStatus();
            Debug.Log("PressButton");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.color = hoveredColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.color = originalColor;
        }

        private void updateReadStatus()
        {
            if (!dialogue.hasBeenRead)
            {
                dialogue.hasBeenRead = true;
                text.color = selectedColor;
            }
        }
    }
}
