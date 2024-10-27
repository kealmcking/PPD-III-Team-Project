using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class MazePuzzlePoint : MonoBehaviour
{
    [SerializeField] PuzzlePointType type;
    [SerializeField] BoxCollider col;
    public BoxCollider Col => col;
    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            if(type == PuzzlePointType.Start && !ScaryMazePuzzleManager.Instance.PuzzleComplete)
            {

                ScaryMazePuzzleManager.Instance.StartGame();
                col.enabled = false;
            }
            else if(type == PuzzlePointType.End)
            {
                ScaryMazePuzzleManager.Instance.WinGame();
            }
        }
    }
    public void HandleReset()
    {
        if(type == PuzzlePointType.Start)
        {
            col.enabled = true;
        }
    }
}

