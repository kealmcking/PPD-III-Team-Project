using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SimonSaysPuzzleManager : MonoBehaviour
{
    public static SimonSaysPuzzleManager Instance;
    [SerializeField] Collider boundsCollider;
    [SerializeField] List<SimonSaysTable> tables = new List<SimonSaysTable>();
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip countdownClip;
    [SerializeField] AudioClip failed;
    [SerializeField] float timeBetweenAnswers = .5f;
    public float TimeBetweenAnswers => timeBetweenAnswers;  
    [SerializeField] int numberOfRounds = 3;
    Queue<int> answers = new Queue<int>();
    Queue<int> guesses = new Queue<int>();
    Coroutine countdownRoutine = null;
    Coroutine answersRoutine = null;
    public int currentRound = 1;
    bool puzzleComplete = false;
    public bool PuzzleComplete => puzzleComplete;   
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
        boundsCollider ??= GetComponent<Collider>();
        source ??= GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && countdownRoutine == null && !puzzleComplete)
        countdownRoutine = StartCoroutine(BeginCountDown());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            ResetPuzzleState();
    }
    private IEnumerator BeginCountDown()
    {
        source.clip = countdownClip;
        source.Play();
        yield return new WaitForSeconds(3);
        countdownRoutine = null;
        GameStart();
    }
    private void GameStart()
    {
        GenerateNextRound();
    }
    private void LoseGame()
    {
        source.clip = failed;
        source.Play();
        ResetPuzzleState();
    }
    private void GenerateNextRound()
    {
        answers.Clear(); // Clear answers from previous round

        int answerCount = currentRound * 2; // Calculate required answers

        while (answerCount > 0)
        {
            Debug.Log("The count of the answer: " + answerCount);
            int randNum = Random.Range(0, tables.Count);
            Debug.Log("The number to be added: " + randNum);
            answers.Enqueue(randNum);
            answerCount--;
        }
        if(answersRoutine == null)
        answersRoutine = StartCoroutine(PlayAnswers());
    }

    public void SaveSelection(int choice)
    {
        if (puzzleComplete) return;
        guesses.Enqueue(choice); // Add guess to queue

        var guessArray = guesses.ToArray();
        var answerArray = answers.ToArray();

        for (int i = 0; i < guesses.Count; i++)
        {
            if (guessArray[i] != answerArray[i])
            {
                LoseGame();
                return;
            }
        }

        if (guesses.Count == answers.Count)
        {
            guesses.Clear();
            answers.Clear();

            if (currentRound >= numberOfRounds)
            {
                puzzleComplete = true;
                // Handle puzzle completion here
            }
            else
            {
                currentRound++;
                GenerateNextRound();
            }
        }
    }

    private IEnumerator PlayAnswers()
    {
        // Use a separate list to avoid modifying answers during playback
        var answerList = new List<int>(answers);
        yield return new WaitForSeconds(1);
        foreach (var answer in answerList)
        {
            tables[answer].SkullLight.SetActive(true);
            tables[answer].PlaySound();
            yield return new WaitForSeconds(timeBetweenAnswers);
            tables[answer].SkullLight.SetActive(false);
        }
        answersRoutine = null;
    }

 
    private void ResetPuzzleState()
    {
        StopAllCoroutines();
        currentRound = 1;
        answers.Clear();
        guesses.Clear();
    }
}
