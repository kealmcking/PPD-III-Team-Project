using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Ghost : Suspect,  ICustomizableComponent
{
    AudioSource audioSource;
    SuspectData suspectData;
    [SerializeField] GhostData ghostData;
    public GhostData GhostData => ghostData;
    public SuspectData SuspectData { get { return suspectData; } set { suspectData = value; } }

    [SerializeField] EnableInteractUI interactUI;
    [SerializeField] Collider col;
  
    bool ghostMoaning = false;

    public virtual void Awake()
    {
     
        col ??= GetComponent<SphereCollider>();
        col.isTrigger = true;
        interactUI ??= GetComponent<EnableInteractUI>();

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null )
        {
            Debug.Log("Ghost audio Error");
        } else
        {
            audioSource.clip = audioManager.instance.ghostMoans;
            audioSource.volume = audioManager.instance.ghostDialogueVol;
            audioSource.outputAudioMixerGroup = audioManager.instance.GetSFXAudioMixer();
            audioSource.spatialBlend = 1f;
        }
    }

    private void Update()
    {
        if( ghostMoaning == false)
        {
            StartCoroutine(ghostMoan());
        }
    }

    private IEnumerator ghostMoan()
    {
        ghostMoaning = true;
        audioSource.PlayOneShot(audioManager.instance.ghostMoans, audioManager.instance.ghostDialogueVol);

        yield return new WaitForSeconds(Random.Range(10f, 15f));

        ghostMoaning = false;
    }



    public override void Interact()
    {

        interactUI.ToggleCanvasOff();
        DialogueManager.instance.enableDialogueUI(this);
    }

 
}
