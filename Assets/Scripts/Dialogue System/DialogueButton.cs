using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public class DialogueButton : MonoBehaviour, IPointerClickHandler
    {
        public Dialogue dialogue;
        public TMP_Text text;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            DialogueManager.instance.selectDialogueLine(dialogue);
            Debug.Log("CLICKED");
        }
    }
}
