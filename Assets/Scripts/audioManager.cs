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
    [SerializeField] AudioClip[] footStepWood;
    [SerializeField] AudioClip[] footStepDirt;
    [SerializeField] AudioClip[] footStepStone;
    [Range(0, 1)] public float footStepVol;

    [SerializeField] AudioClip[] jump;
    [Range(0, 1)] public float jumpVol;

    [SerializeField] AudioClip[] landWood;
    [SerializeField] AudioClip[] landDirt;
    [SerializeField] AudioClip[] landStone;
    [Range(0, 1)] public float landVol;

    [SerializeField] AudioClip[] crouchDown;
    [SerializeField] AudioClip[] crouchUp;
    [Range(0, 1)] public float crouchVol;

    [Header("------------------------- Puzzle SFX")]
    [SerializeField] AudioClip[] puzzleMove;
    [SerializeField] AudioClip[] puzzleHit;
    [SerializeField] AudioClip puzzleWin;
    [SerializeField] AudioClip puzzleLose;
    [SerializeField] AudioClip[] puzzleMisc;
    [Range(0, 1)] public float puzzleVol;

    [Header("------------------------- UI SFX")]
    [SerializeField] AudioClip UIOpen;
    [SerializeField] AudioClip UIClose;
    [SerializeField] AudioClip UIClick;
    [SerializeField] AudioClip UIWin;
    [SerializeField] AudioClip UILose;
    [SerializeField] AudioClip UIMusic;
    [Range(0, 1)] public float UIVol;
    [Range(0, 1)] public float UIMusicVol;

    [Header("------------------------- Inventory SFX")]
    [SerializeField] AudioClip invOpen;
    [SerializeField] AudioClip invClose;
    [SerializeField] AudioClip invPickup;
    [SerializeField] AudioClip invDrop;
    [SerializeField] AudioClip invEquip;
    [Range(0, 1)] public float invVol;

    [Header("------------------------- Crafting SFX")]
    [SerializeField] AudioClip craftOpen;
    [SerializeField] AudioClip craftClose;
    [SerializeField] AudioClip[] craftItem;
    [SerializeField] AudioClip[] craftFail;
    [Range(0, 1)] public float craftVol;

    [Header("------------------------- Dialogue SFX")]
    [SerializeField] AudioClip[] dialogueMutter;
    [SerializeField] AudioClip[] dialogueTalkF1; //Female 1
    [SerializeField] AudioClip[] dialogueTalkF2; //Female 2
    [SerializeField] AudioClip[] dialogueTalkM1; //Male 1
    [SerializeField] AudioClip[] dialogueTalkM2; //Male 2
    //[SerializeField] AudioClip dialogueLieIndicator;
    [SerializeField] AudioClip dialogueClose;
    [Range(0, 1)] public float dialogueVol;

    [SerializeField] AudioClip[] ghostMoans;
    [Range(0, 1)] public float ghostDialogueVol;

    [Header("------------------------- Killer SFX")]
    [SerializeField] AudioClip killerChaseMusic;
    [Range(0, 1)] public float killerMusicVol;

    [SerializeField] AudioClip[] killerKill;
    [Range(0, 1)] public float killerVol;

    [Header("------------------------- Misc SFX")]
    [SerializeField] AudioClip clueFound;
    [Range(0, 1)] public float clueVol;


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
        SFX.PlayOneShot(clip, vol);
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
