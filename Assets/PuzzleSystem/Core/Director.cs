using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Director : MonoBehaviour
{
    //wait here and listen for event to start or stop the timer
    //Listen for event that updates what objectives have been guessed  
    //send the array of muder objects for decision making/UI
    public static Action<Suspect> FinalDay; //Allows you to change suspects(killer) functioning
    public static Action<List<Puzzle>> SendPuzzles;
    public static Action<List<Lore>> SendLore;
    public static Action<BaseClueData> SendFoundClue;
    public static Action<List<MurderRoom>> SendMurderRooms;
    public static Action<List<Suspect>> SendSuspects;
    public static Action<List<MurderWeapon>> SendMurderWeapons;
    public static Action<List<MurderMotive>> SendMurderMotives;
    public static Action<GameSelection> SendGameSelection;
    public static Action<int> SendDayCounter;
    public static Action TimerStart;
    public static Action TimerStop;
     
    [SerializeField,Tooltip("This is a list of the game winning conditions, (It should never change)")] ConditionConfig[] globalWinConditions;
    [SerializeField,Tooltip("This contains the list of where the murder can take place. " +
        "It is also used to update the UI for instance when guessing the room in which the murder takes place")] List<MurderRoom> rooms;
    [SerializeField,Tooltip("This contains the list of the murder weapons. " +
        "It is also used to update the UI for instance when guessing the weapon which was used for the murder")] List<MurderWeapon> weapons;
    [SerializeField, Tooltip("This contains the list of the motives. " +
      "It is also used to update the UI for instance when guessing the motive which was used for the murder")] List<MurderMotive> motives;
    [SerializeField,Tooltip("This represents all the possible cases available for this level")] List<Case> cases;
    [Range(1,60),SerializeField, Tooltip("The length each day (round) should be. The number placed here is multiplied by 60 to represent one minute " +
        "Example: 5 = 5 minutes")] int dayLength ;
    private Puzzle[] scenePuzzles;
    private Lore[] sceneLore;
    private List<Suspect> suspects;
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
    public List<Case> Cases => cases;
    public List<MurderMotive> Motives => motives;
    public List<MurderRoom> Rooms => rooms;  
    public List<MurderWeapon> Weapons => weapons;
    public GameSelection GameSelection => gameSelection;
    public void Start()
    {
        suspects = FindObjectsByType<Suspect>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        scenePuzzles = FindObjectsByType<Puzzle>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        sceneLore = FindObjectsByType<Lore>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        gameSelection = new GameSelection(suspects,rooms,weapons,cases,motives);
        cController = new ClueController();
        List<Puzzle> activeP = new List<Puzzle>();
        List<Lore> activeL = new List<Lore>();
        if(scenePuzzles.Length > 0 && sceneLore.Length > 0)
        {
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
        }
        if(suspects.Count() > 0)
        DayCounter = suspects.Count();
        DayLength = dayLength * 60f;

        SendGameSelection.Invoke(gameSelection);
        SendMurderWeapons.Invoke(weapons);
        SendMurderRooms.Invoke(rooms);
        SendMurderMotives.Invoke(motives);
        SendSuspects.Invoke(suspects);
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
            SendFoundClue.Invoke(clue);
        }       
    }
}
