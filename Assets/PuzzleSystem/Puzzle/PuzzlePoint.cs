using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class PuzzlePoint : MonoBehaviour
{
    [SerializeField] PuzzlePointType type;
    [SerializeField] BoxCollider col;
    public BoxCollider Col => col;
    private void Awake()
    {
        col ??= GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            if(type == PuzzlePointType.Start)
            {
                IcePuzzleManager.Instance.StartPuzzle();
                col.enabled = false;
            }
            else if (type == PuzzlePointType.Restart)
            {
               other.gameObject.transform.position = IcePuzzleManager.Instance.RestartPoint.position;
                other.gameObject.transform.rotation = IcePuzzleManager.Instance.RestartPoint.rotation;
               IcePuzzleManager.Instance.ResetPuzzle();
            }
            else if(type == PuzzlePointType.End)
            {
                IcePuzzleManager.Instance.ResetPuzzle();
            }
        }
    }
}
public enum PuzzlePointType
{
    None,
    Start,
    End,
    Restart,
}
