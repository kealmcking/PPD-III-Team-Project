using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
[RequireComponent(typeof(AudioSource))]
public class ScaryMazePuzzleManager : MonoBehaviour
{
    public static Action ResetGameEvent;
    public static ScaryMazePuzzleManager Instance;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip countdownClip;
    [SerializeField] AudioClip clockClip;
    [SerializeField] AudioClip failed;
    [SerializeField] MazePuzzlePoint startPoint;
    [SerializeField] Transform resetPoint;
    [SerializeField] GameObject barrier;
    [SerializeField] List<Transform> teleportPoints = new List<Transform>();
    [SerializeField] float timeUntilLose = 60f;
    float _time;

    public bool timerOn;
    Coroutine countdownRoutine = null;
    bool puzzleComplete = false;
    public bool PuzzleComplete => puzzleComplete;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        if(source is null)
        source = GetComponent<AudioSource>();
        _time = timeUntilLose;
    }
    private void OnEnable()
    {
        if (startPoint != null)
        {
            ResetGameEvent += startPoint.HandleReset;
        }
    }
    private void OnDisable()
    {
        if (startPoint != null)
        {
            ResetGameEvent -= startPoint.HandleReset;
        }
    }
    private void Update()
    {
        UpdateTimer(); 
    }
    public void UpdateTimer()
    {
        if (timerOn && _time > 0)
        {
            _time -= Time.deltaTime;
        }
        if (_time <= 0)
        {
            _time = 0;
            LoseGame(); // Trigger loss when time runs out
        }
    }
    private IEnumerator BeginCountDown()
    {
        source.clip = countdownClip;
        source.Play();
        yield return new WaitForSeconds(3);
        timerOn = true;
        source.clip = clockClip;
        source.loop = true;
        source.spatialBlend = 0f;
        source.Play();
        barrier.SetActive(false);
        countdownRoutine = null;
    }
    public void StartGame()
    {
        if (countdownRoutine == null)
            countdownRoutine = StartCoroutine(BeginCountDown());
    }
    public void SendToNewPoint(Transform playerTransform)
    {
        if(teleportPoints.Count > 0)
        {
            CharacterController controller = playerTransform.GetComponent<CharacterController>();
            controller.enabled = false;
            Transform point = Randomizer.GetRandomizedObjectFromList(teleportPoints);
            playerTransform.position = point.position;
            playerTransform.rotation = point.rotation;
            controller.enabled = true;
        }
    }
    public void ResetGame()
    {
        GameObject player = GameObject.FindWithTag("Player");
        barrier.SetActive(true);
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.position = resetPoint.position;
        player.transform.rotation = resetPoint.rotation;
        controller.enabled = true;
        timerOn = false;
        _time = timeUntilLose;
        source.loop = false;
        source.clip = failed;
        source.Play();
    }
    public void WinGame()
    {
        source.loop = false;
        puzzleComplete = true;
    }
    public void LoseGame()
    {
        
        ResetGame();
        ResetGameEvent?.Invoke();
    }
}
