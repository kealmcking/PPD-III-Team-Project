using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCatcher : MonoBehaviour
{
    private audioManager audioManager;
    private playerController playerController;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<playerController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<audioManager>();
    }

    public void ThrowEvent()
    {
        EventSheet.ThrowAnimationEvent?.Invoke();
    }
    public void ItemColliderToggle()
    {
        EventSheet.ItemColliderAnimationEvent?.Invoke();
    }

    private void footStep()
    {
        if (!playerController.GetCrouch() && !playerController.GetSprint())          //Walking
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepWalkVol);
        }
        else if (!playerController.GetCrouch() && playerController.GetSprint())     //Sprinting
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepRunVol);
        }
        else if (playerController.GetCrouch() && !playerController.GetSprint())     //Crouching
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepCrouchVol);
        } else                                                                      //Other/Error
        {
            audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepWalkVol);
        }
    }
}


