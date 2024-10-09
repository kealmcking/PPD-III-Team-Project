using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public static audioManager instance;
    private bool musicPaused;
    private float origMusicVol;
    public enum floorType { Wood, Dirt, Stone }
    private floorType currentFloor;

    [Header("------------------------- Audio Sources")]
    [SerializeField] AudioSource SFX;

    [Header("------------------------- Movement SFX")]
    [SerializeField] public AudioClip[] footStepWood;
    [SerializeField] public AudioClip[] footStepDirt;
    [SerializeField] public AudioClip[] footStepStone;
    [Range(0, 1)] public float footStepVol;

    [SerializeField] public AudioClip[] jump;
    [Range(0, 1)] public float jumpVol;

    [SerializeField] public AudioClip[] landWood;
    [SerializeField] public AudioClip[] landDirt;
    [SerializeField] public AudioClip[] landStone;
    [Range(0, 1)] public float landVol;

    [SerializeField] public AudioClip[] crouchDown;
    [SerializeField] public AudioClip[] crouchUp;
    [Range(0, 1)] public float crouchVol;

    [Header("------------------------- Puzzle SFX")]
    [SerializeField] public AudioClip[] puzzleMove;
    [SerializeField] public AudioClip[] puzzleHit;
    [SerializeField] public AudioClip puzzleWin;
    [SerializeField] public AudioClip puzzleLose;
    [SerializeField] public AudioClip[] puzzleMisc;
    [Range(0, 1)] public float puzzleVol;

    [Header("------------------------- UI SFX")]
    [SerializeField] public AudioClip UIOpen;
    [SerializeField] public AudioClip UIClose;
    [SerializeField] public AudioClip UIClick;
    [SerializeField] public AudioClip UIWin;
    [SerializeField] public AudioClip UILose;
    [SerializeField] public AudioClip UIMusic;
    [Range(0, 1)] public float UIVol;
    [Range(0, 1)] public float UIMusicVol;

    [Header("------------------------- Inventory SFX")]
    [SerializeField] public AudioClip invOpen;
    [SerializeField] public AudioClip invClose;
    [SerializeField] public AudioClip invPickup;
    [SerializeField] public AudioClip invDrop;
    [SerializeField] public AudioClip invEquip;
    [Range(0, 1)] public float invVol;

    [Header("------------------------- Crafting SFX")]
    [SerializeField] public AudioClip craftOpen;
    [SerializeField] public AudioClip craftClose;
    [SerializeField] public AudioClip[] craftItem;
    [SerializeField] public AudioClip[] craftFail;
    [Range(0, 1)] public float craftVol;

    [Header("------------------------- Dialogue SFX")]
    [SerializeField] public AudioClip[] dialogueMutter;
    [SerializeField] public AudioClip[] dialogueTalkF1; //Female 1
    [SerializeField] public AudioClip[] dialogueTalkF2; //Female 2
    [SerializeField] public AudioClip[] dialogueTalkM1; //Male 1
    [SerializeField] public AudioClip[] dialogueTalkM2; //Male 2
    //[SerializeField] AudioClip dialogueLieIndicator;
    [SerializeField] public AudioClip dialogueClose;
    [Range(0, 1)] public float dialogueVol;

    [SerializeField] public AudioClip[] ghostMoans;
    [Range(0, 1)] public float ghostDialogueVol;

    [Header("------------------------- Killer SFX")]
    [SerializeField] public AudioClip killerChaseMusic;
    [Range(0, 1)] public float killerMusicVol;

    [SerializeField] public AudioClip[] killerKill;
    [Range(0, 1)] public float killerVol;

    [Header("------------------------- Misc SFX")]
    [SerializeField] public AudioClip clueFound;
    [Range(0, 1)] public float clueVol;
    [SerializeField] public AudioClip interactSFX;
    [Range(0, 1)] public float interactVol;
    [SerializeField] public AudioClip sleeping;
    [Range(0, 1)] public float sleepVol;

    [SerializeField] public AudioClip errorAudio;



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

    public void PlaySFX(AudioClip clip, float vol = 0.5f)
    {
        if (clip == null)
        {
            SFX.PlayOneShot(errorAudio, vol);
        } else
        {
            SFX.PlayOneShot(clip, vol);
        }
    }

    public void MusicFade()
    {
        //Turn Music down
        //Pause Music
        //Turn musicPaused to opposite
    }

    public floorType GetFloorType()
    {
        return currentFloor;
    }
    private void SetFloorType(floorType curfloor)
    {
        currentFloor = curfloor;
    }
}
