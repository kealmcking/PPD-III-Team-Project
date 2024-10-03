using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [Header("------------------------- Audio Sources")]
    [SerializeField] AudioSource SFX;

    [Header("------------------------- Movement SFX")]
    [SerializeField] AudioClip[] footStep;
    [Range(0, 1)] public float footStepVol;

    [SerializeField] AudioClip[] jump;
    [Range(0, 1)] public float jumpVol;

    [SerializeField] AudioClip[] crouchDown;
    [SerializeField] AudioClip[] crouchUp;
    [Range(0, 1)] public float crouchVol;
}
