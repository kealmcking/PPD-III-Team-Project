using System;
using Unity.VisualScripting;
using UnityEngine;
//make editor script for Lore when make Lore button is pressed a window will pop-up that asks which case/motive you want to add it to then press ok.
//This will create a prefab and add the prefab to the correct scriptable object for this puzzle
public class Lore : MonoBehaviour,IInteractable
{
    public static Action<IInteractable> SendLore;
    [SerializeField, Tooltip("Place a description explaining the lore here")] Description description;
    [SerializeField, Tooltip("represents icon image")] protected Sprite icon;
    public void Interact()
    {
        SendLore.Invoke(this);
    }

    public Payload GetPayload()
    {
        return new Payload { isEmpty = false, lore = this };
    }
}
