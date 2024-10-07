using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Manages the puzzle system and determines the initial game state.
/// </summary>
public class Director : MonoBehaviour
{
    //wait here and listen for event to start or stop the timer
    //Listen for event that updates what objectives have been guessed  
    //send the array of muder objects for decision making/UI

    public static Action<List<Puzzle>> SendPuzzles;
    public static Action<List<Lore>> SendLore;
    public static Action<BaseClueData> SendFoundClue;

    public static Action<List<MurderRoom>> SendMurderRooms;
    public static Action<List<MurderWeapon>> SendMurderWeapons;
    public static Action<List<MurderMotive>> SendMurderMotives;
    public static Action<List<Suspect>> SendSuspects;

    public static Action<GameSelection> SendGameSelection;
 

    [SerializeField,Tooltip("This is a list of the game winning conditions, (It should never change)")] ConditionConfig[] globalWinConditions;
    [SerializeField,Tooltip("This contains the list of where the murder can take place. " +
        "It is also used to update the UI for instance when guessing the room in which the murder takes place")] List<MurderRoom> rooms;
    [SerializeField,Tooltip("This contains the list of the murder weapons. " +
        "It is also used to update the UI for instance when guessing the weapon which was used for the murder")] List<MurderWeapon> weapons;
    [SerializeField, Tooltip("This contains the list of the motives. " +
      "It is also used to update the UI for instance when guessing the motive which was used for the murder")] List<MurderMotive> motives;
    [SerializeField,Tooltip("This represents all the possible cases available for this level")] List<Case> cases;
    [SerializeField] List<Puzzle> scenePuzzles;
    private List<Lore> sceneLore;
    private List<Suspect> suspects;
    private GameSelection gameSelection;
    private ClueController cController;
    private PuzzleController pController;
    private LoreController lController;
    
    public List<Case> Cases { get { return cases; } private set { cases = value; } }
    public List<MurderRoom> Rooms { get { return rooms; } private set { rooms = value; } }
    public List<MurderWeapon> Weapons { get { return weapons; } private set { weapons = value; } }
    public List<MurderMotive> Motives { get { return motives; } private set { motives = value; } }
    public GameSelection GameSelection => gameSelection;
    public void Start()
    {       
        suspects = FindObjectsByType<Suspect>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        scenePuzzles = FindObjectsByType<Puzzle>(FindObjectsInactive.Include,FindObjectsSortMode.None).ToList();
        sceneLore = FindObjectsByType<Lore>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        gameSelection = new GameSelection(suspects,rooms,weapons,cases,motives);
        cController = new ClueController();
        List<Puzzle> activeP = new List<Puzzle>();
        List<Lore> activeL = new List<Lore>();
        if (scenePuzzles.Count > 0)
        {
            foreach (Puzzle puzzle in gameSelection.GetCase().Puzzles)
            {
                Puzzle sPuzzle = scenePuzzles.FirstOrDefault(sp => sp.Equals(puzzle));
                if (sPuzzle != null)
                {
                    sPuzzle.gameObject.SetActive(true);
                    activeP.Add(sPuzzle);
                }
            }
        }
        if (sceneLore.Count > 0)
        {
            foreach (Lore lore in gameSelection.GetCase().Lore)
            {
                Lore sLore = sceneLore.FirstOrDefault(sp => sp.Equals(lore));
                if (sLore != null)
                {
                    sLore.gameObject.SetActive(true);
                    activeL.Add(sLore);
                }
            }
            pController = new PuzzleController(activeP);
            lController = new LoreController(activeL);
        }
          
        
        

        /*SendMurderMotives.Invoke(motives);
        SendMurderRooms.Invoke(rooms);
        SendMurderWeapons.Invoke(weapons);
        SendSuspects.Invoke(suspects);*/
        //SendGameSelection.Invoke(gameSelection);
    }
  
    
    private class PuzzleController
    {
        private List<Puzzle> activePuzzles = new List<Puzzle>();
        public List<Puzzle> ActivePuzzles => activePuzzles;
        public PuzzleController(List<Puzzle> puzzles)
        {
            activePuzzles = puzzles;
            //SendPuzzles.Invoke(activePuzzles);
        }
    }
    private class LoreController
    {
        private List<Lore> activeLore = new List<Lore>();
        public List<Lore> ActiveLore => activeLore;
        public LoreController(List<Lore> lores)
        {
            activeLore = lores;
            //SendLore.Invoke(activeLore);
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
