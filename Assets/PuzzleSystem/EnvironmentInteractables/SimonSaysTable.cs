using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SimonSaysTable : EnvironmentInteractable
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] GameObject skullLight;
    [SerializeField] int id;
    public int ID => id;
    public GameObject SkullLight => skullLight;
    private void Awake()
    {
        source ??= GetComponent<AudioSource>();
        source.clip = clip;
        skullLight.SetActive(false);
    }
    public override void Interact()
    {
        Flash();
        SimonSaysPuzzleManager.Instance.SaveSelection(ID);
    }
    public void PlaySound()
    {
        source.Play();
    }
    public void Flash()
    {
        StartCoroutine(FlashCandleLight());
    }
    IEnumerator FlashCandleLight()
    {
        PlaySound();
        skullLight.SetActive(true);
        yield return new WaitForSeconds(SimonSaysPuzzleManager.Instance.TimeBetweenAnswers);
        skullLight.SetActive(false);
    }
}
