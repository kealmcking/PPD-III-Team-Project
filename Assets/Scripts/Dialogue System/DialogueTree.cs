using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueTree")]
public class DialogueTree : ScriptableObject
{
    public List<Dialogue> dialogues;

    public NPC speaker;
}
