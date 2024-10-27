using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource),typeof(Rigidbody))]
public class IceBlock : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip breakSplash;
    [SerializeField] Vector3 startPosition;
    [SerializeField] Quaternion startRotation;
    [SerializeField] bool isBroke;
    [SerializeField] Collider col;
    Coroutine breakRoutine;
    public bool IsBroke => isBroke;
    private void Awake()
    {
        rb ??= GetComponent<Rigidbody>();
        audioSource ??= GetComponent<AudioSource>();
        rb.isKinematic = true;
        rb.useGravity = false;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void BreakOff()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        isBroke = true;
        col.enabled = false;
        if (breakRoutine != null)
        {
            StopCoroutine(breakRoutine);
            breakRoutine = null;
        }
    }
    public void ResetBlock()
    {
        rb.isKinematic= true;
        rb.useGravity= false;
        transform.position = startPosition;
        transform.rotation = startRotation;
        isBroke = false;
        col.enabled = true;
        if (breakRoutine != null)
        {
            StopCoroutine(breakRoutine);
            breakRoutine = null;
        }
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("ResetCollider"))
        {
            audioSource.clip = breakSplash;
            audioSource.Play();
        }
        if(other != null && other.CompareTag("Player"))
        {
            if(breakRoutine == null)
            breakRoutine = StartCoroutine(Break());
        }
    }
    IEnumerator Break()
    {
        yield return new WaitForSeconds(IcePuzzleManager.Instance.TimeBetweenBlockDrops);
        BreakOff();
        breakRoutine = null;
    }
}
