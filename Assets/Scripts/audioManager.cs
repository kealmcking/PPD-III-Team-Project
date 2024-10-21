using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{

    #region Variables/Audio
    public static audioManager instance;
    private bool menuPaused;
    private float origMusicVol;
    public enum floorType { Wood, Dirt, Stone }
    private floorType currentFloor;

    [Header("------------------------- Audio Sources")]
    [SerializeField] AudioSource SFX;
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource Menu;

    [Header("------------------------- Movement SFX")]
    [SerializeField] public AudioClip[] footStepWood;
    [SerializeField] public AudioClip[] footStepDirt;
    [SerializeField] public AudioClip[] footStepStone;
    [Range(0, 1)] public float footStepWalkVol;
    [Range(0, 1)] public float footStepRunVol;
    [Range(0, 1)] public float footStepCrouchVol;

    [SerializeField] public AudioClip[] crouchDown;
    [SerializeField] public AudioClip[] crouchUp;
    [Range(0, 1)] public float crouchVol;

    [SerializeField] public AudioClip pickUp;
    [Range(0, 1)] public float pickUpVol;

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
    [Range(0, 1)] public float UIVol;

    [Header("------------------------- Inventory SFX")]
    [SerializeField] public AudioClip invOpen;
    [SerializeField] public AudioClip invClose;
    [SerializeField] public AudioClip invPickup;
    [SerializeField] public AudioClip invDrop;
    [SerializeField] public AudioClip invEquip;
    [Range(0, 1)] public float invVol;

    [Header("------------------------- Dialogue SFX")]
    [SerializeField] public AudioClip[] dialogueMutter;
    [Range(0, 1)] public float dialogueVol;

    [SerializeField] public AudioClip ghostMoans;
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
    [SerializeField] public AudioClip flashlight;
    [Range(0, 1)] public float flashlightVol;
    [SerializeField] public AudioClip errorAudio;

    [Header("------------------------- Misc Variables")]
    [Range(0, 3)] public float Fadetime;

    #endregion

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

        origMusicVol = Music.volume;
        menuPaused = false;
    }

    public void PlaySFX(AudioClip clip, float vol = 0.5f)
    {
        //Play SFX passed in
        if (clip == null)
        {
            SFX.PlayOneShot(errorAudio, vol);
        } else
        {
            SFX.PlayOneShot(clip, vol);
        }
    }

    public void PauseSounds()
    {
        if (menuPaused) //If true unpause all sounds
        {
            //Stop Menu Music
            Menu.Stop();

            //Unpause game sounds
            Music.UnPause();
            SFX.UnPause();

            menuPaused = false;
        }
        else   //If false pause all sounds
        {
            //Pause game sounds
            if (Music.isPlaying) { Music.Pause(); }
            if (SFX.isPlaying) { SFX.Pause(); }

            menuPaused = true;

            //Play Menu Music
            Menu.Play();
        }
    }

    #region FloorTypes
    public floorType GetFloorType()
    {
        return currentFloor;
    }
    private void SetFloorType(floorType curfloor)
    {
        currentFloor = curfloor;
    }
    #endregion
}
