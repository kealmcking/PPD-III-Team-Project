using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class IcePuzzleManager : MonoBehaviour
{
    public static IcePuzzleManager Instance;
    [SerializeField] List<IceBlock> iceBlocks = new List<IceBlock>();
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip tickSound;
    [SerializeField] AudioClip resetSound;
    [SerializeField] Transform restartPoint;
    [SerializeField] float timeBetweenBlockDrops = .8f;
    [SerializeField] PuzzlePoint startPoint;
    public bool PuzzleComplete { get; set; } = false;
    Coroutine blockRoutine;
    public Transform RestartPoint => restartPoint;
    public float TimeBetweenBlockDrops => timeBetweenBlockDrops;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        source ??= GetComponent<AudioSource>();
    }

    public void StartPuzzle()
    {
        source.loop = true;
        source.clip = tickSound;
        source.spatialBlend = 0f;
        source.Play();
        if(blockRoutine == null)
        blockRoutine = StartCoroutine(DropBlocks());
    }
    IEnumerator DropBlocks()
    {
        for(int i = 0; i < iceBlocks.Count; i++)
        {
            if (iceBlocks[i].IsBroke) continue;
            yield return new WaitForSeconds(timeBetweenBlockDrops);
            iceBlocks[i].BreakOff();
        }
        blockRoutine = null;
        ResetPuzzle();
        
    }
    public void ResetPuzzle()
    {
        source.loop = false;
        source.clip = resetSound;
        source.Play();
        if (blockRoutine != null)
        {
            StopCoroutine(blockRoutine);
            blockRoutine = null;
        }
        foreach (var block in iceBlocks)
        {
            block.ResetBlock();
        }
        startPoint.Col.enabled = true;
    }
    public void WinGame()
    {
        PuzzleComplete = true;  
    }
}
