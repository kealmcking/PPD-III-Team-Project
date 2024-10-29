using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpawn : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    public void PlayFX()
    {
        Debug.Log("Play effect");
        _particleSystem.Play();
    }
}
