using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour, IInteractable, ICustomizableComponent
{
    SuspectData suspectData;
    [SerializeField] GhostData ghostData;
    public GhostData GhostData => ghostData;
    public SuspectData SuspectData { get { return suspectData; } set { suspectData = value; } }
    public void Interact()
    {

    }
    public GameObject GetObject()
    {
        return gameObject;
    }

}
