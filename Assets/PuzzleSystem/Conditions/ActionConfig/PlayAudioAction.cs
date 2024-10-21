using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayAudioAction", menuName = "PuzzleSystem/ActionConfigs/PlayAudioAction")]
public class PlayAudioAction : ActionConfig
{
    [SerializeField] AudioClip clip;
    public override void RunAction(Condition condition)
    {
        AudioSource source = condition.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

}
