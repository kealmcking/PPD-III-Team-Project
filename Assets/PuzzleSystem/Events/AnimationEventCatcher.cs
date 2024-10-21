using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCatcher : MonoBehaviour
{
    private audioManager audioManager;

    private void Awake()
    {
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
        audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepWalkVol);

        //if (!_isCrouching && !_isSprinting)          //Walking
        //{
        //    audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepWalkVol);
        //}
        //else if (!_isCrouching && _isSprinting)   //Sprinting
        //{
        //    audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepRunVol);
        //}
        //else if (!_isSprinting && _isCrouching)   //Crouching
        //{
        //    audioManager.PlaySFX(audioManager.footStepWood[UnityEngine.Random.Range(0, audioManager.footStepWood.Length)], audioManager.footStepCrouchVol);
        //}
    }
}


