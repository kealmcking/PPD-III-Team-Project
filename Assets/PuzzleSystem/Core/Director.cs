using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Director : MonoBehaviour
{
    //wait here and listen for event to start or stop the timer

    public static Action<Suspect> FinalDay;
    public static Action<List<Puzzle>> SendPuzzles;
    public static Action<List<Lore>> SendLore;
    public static Action<List<BaseClueData>> SendFoundClues;
    public static Action<int> SendDayCounter;
    public static Action TimerStart;
    public static Action TimerStop;

    //Listen for event that updates what objectives have been guessed    
    [SerializeField,Tooltip("This is a list of the game winning conditions, (It should never change)")] ConditionConfig[] sceneConditions;
    [SerializeField,Tooltip("This contains the list of names where the murder can take place. " +
        "It is also used to update the UI for instance when guessing the room in which the murder takes place")] Room[] rooms;
    [SerializeField,Tooltip("This contains the list of names of the murder weapons. " +
        "It is also used to update the UI for instance when guessing the weapon which was used for the murder")] MurderWeapon[] weapons;
    [SerializeField,Tooltip("This represents all the possible motives(cases) available for this level")] Motive[] motives;
    [Range(1,60),SerializeField, Tooltip("The length each day (round) should be. The number placed here is multiplied by 60 to represent one minute " +
        "Example: 5 = 5 minutes")] int dayLength ;
    private Puzzle[] scenePuzzles;
    private Lore[] sceneLore;
    private Suspect[] suspects;
    private GameSelection gameSelection;
    private ClueController cController;
    private PuzzleController pController;
    private LoreController lController;
    private float DayLength;
    public int DayCounter { 
        get { return DayCounter; } 
        private set { DayCounter = value;
            SendDayCounter.Invoke(DayCounter);
            if (DayCounter <= 1) 
            {
                FinalDay.Invoke(suspects.FirstOrDefault(s => s.IsKiller));            
            } 
        } 
    }
    private float currentTimer = 0;
    private bool isTimerGoing = false;
    public GameSelection GameSelection => gameSelection;
    public void Start()
    {
        suspects = FindObjectsByType<Suspect>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        scenePuzzles = FindObjectsByType<Puzzle>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        sceneLore = FindObjectsByType<Lore>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        gameSelection = new GameSelection(suspects,rooms,weapons,motives);
        cController = new ClueController();
        List<Puzzle> activeP = new List<Puzzle>();
        List<Lore> activeL = new List<Lore>();
        foreach (Puzzle puzzle in gameSelection.GetCase().Puzzles)
        {
            Puzzle sPuzzle = scenePuzzles.FirstOrDefault(sp => sp.Equals(puzzle));
            if (sPuzzle != null)
            {
                puzzle.gameObject.SetActive(true);
                activeP.Add(sPuzzle);
            }          
        }
        foreach (Lore lore in gameSelection.GetCase().Lore)
        {
            Lore sLore = sceneLore.FirstOrDefault(sp => sp.Equals(lore));
            if (sLore != null)
            {
                lore.gameObject.SetActive(true);
                activeL.Add(sLore);
            }
        }
        pController = new PuzzleController(activeP);
        lController = new LoreController(activeL);
        DayCounter = suspects.Count();
        DayLength = dayLength * 60f;
    }
    public void Update()
    {
        if (isTimerGoing)
        {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0)
            {
                currentTimer = 0;
                DayCounter--;
                StopTimer();
            }
        }
    }
    private void StartTimer()
    {
        currentTimer = dayLength;
        isTimerGoing = true;
    }
    private void StopTimer()
    {
        isTimerGoing=false;
    }
    private void UpdateSceneObjective()
    {

    }

    private class PuzzleController
    {
        private List<Puzzle> activePuzzles = new List<Puzzle>();
        public List<Puzzle> ActivePuzzles => activePuzzles;
        public PuzzleController(List<Puzzle> puzzles)
        {
            activePuzzles = puzzles;
            SendPuzzles.Invoke(activePuzzles);
        }
    }
    private class LoreController
    {
        private List<Lore> activeLore = new List<Lore>();
        public List<Lore> ActiveLore => activeLore;
        public LoreController(List<Lore> lores)
        {
            activeLore = lores;
            SendLore.Invoke(activeLore);
        }
    }
    private class ClueController
    {
        private List<BaseClueData> foundClues = new List<BaseClueData>();
        public ClueController(){ }
        public void AddClue(BaseClueData clue)
        { 
            foundClues.Add(clue);
            SendFoundClues.Invoke(foundClues);
        }       
    }
}
