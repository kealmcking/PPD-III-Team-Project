using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PuzzlePlatformActivation : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSet;
    [SerializeField] AudioClip activateSound;
    [SerializeField] AudioClip deactivateSound;
    [SerializeField] AudioSource source;
    private bool setNonBlocked = true;
    private void OnEnable()
    {
  
        EventSheet.GateConditionStatus += BlockPlatform;
    }
    private void OnDisable()
    {
       
        EventSheet.GateConditionStatus -= BlockPlatform;
    }
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
        if(controller!= null && setNonBlocked)
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
        if (controller != null )
        {
            foreach (GameObject obj in objectsToSet)
            {
                obj.SetActive(false);
               
            }
            source.clip = deactivateSound;
            source.Play();
        }

    }
    private void BlockPlatform(bool value)
    {
        setNonBlocked = value;  
    }
}
