using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PuzzlePlatformActivation : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSet;
    [SerializeField] AudioClip activateSound;
    [SerializeField] AudioClip deactivateSound;
    [SerializeField] AudioSource source;
    private void Start()
    {
        foreach (GameObject obj in objectsToSet)
        {
            obj.SetActive(false);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out playerController controller);
        if(controller!= null)
        {
            source.clip = activateSound;
            source.Play();
            foreach (GameObject obj in objectsToSet)
            {
                obj.SetActive(true);
               
            }
           
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out playerController controller);
        if (controller != null)
        {
            foreach (GameObject obj in objectsToSet)
            {
                obj.SetActive(false);
               
            }
            source.clip = deactivateSound;
            source.Play();
        }

    }
}
