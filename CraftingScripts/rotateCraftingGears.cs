using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCraftingGears : MonoBehaviour
{
    [SerializeField] GameObject topCog;
    [SerializeField] GameObject leftCog;
    [SerializeField] GameObject rightCog;

    public float lerpSpeed = 0.2f;

    private Quaternion targetRotation;

    void Update()
    {
        topCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        leftCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        rightCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }

    private void Start()
    {
        targetRotation = Quaternion.Euler(0, 0, 180);
    }
}
