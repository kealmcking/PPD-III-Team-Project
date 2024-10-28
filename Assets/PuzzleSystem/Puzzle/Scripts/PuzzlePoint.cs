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
        col = GetComponent<BoxCollider>();
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
               
                    playerController player = other.GetComponent<playerController>();
                    if (player != null)
                    {
                    player.slipperyFactor = 1f;
                    player.isSlipping = false;
                    }
                
                CharacterController controller = other.GetComponent<CharacterController>();
                controller.enabled = false;
                other.gameObject.transform.position = IcePuzzleManager.Instance.RestartPoint.position;
                other.gameObject.transform.rotation = IcePuzzleManager.Instance.RestartPoint.rotation;
                controller.enabled = true;
                IcePuzzleManager.Instance.ResetPuzzle();
            }
            else if(type == PuzzlePointType.End)
            {
                IcePuzzleManager.Instance.WinGame();
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
