using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Director : MonoBehaviour
{
    //Listen for event that updates what objectives have been guessed    
   
    [SerializeField,Tooltip("This is a list of the game winning conditions, (It should never change)")] ConditionConfig[] sceneConditions;
    [SerializeField,Tooltip("This contains the list of names where the murder can take place. " +
        "It is also used to update the UI for instance when guessing the room in which the murder takes place")] string[] rooms;//change this to a scriptable object
    [SerializeField,Tooltip("This contains the list of names of the murder weapons. " +
        "It is also used to update the UI for instance when guessing the weapon which was used for the murder")] string[] weapons;//change this to a scriptable object
    [SerializeField,Tooltip("This represents all the possible motives(cases) available for this level")] Motive[] motives;
    [SerializeField,Tooltip("This controls how many puzzles will be pulled/ made available per day." +
        "Consequently it determines how many puzzles to get from the randomizer")] int puzzleQuantity = 3;
    private Puzzle[] scenePuzzles;
    private Lore[] sceneLore;
    private Suspect[] suspects;

    private ClueController cController;
    private PuzzleController pController;
    private LoreController lController;
    //Timer for each round
    //Tracks days
    //Initiates puzzles which in turn pull the objectives for the puzzle

    public void UpdateSceneObjective()
    {

    }
    private GameSelection gameSelection;
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
                sPuzzle.gameObject.SetActive(true);
                activeP.Add(sPuzzle);
            }          
        }
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

    private class PuzzleController
    {
        private List<Puzzle> activePuzzles = new List<Puzzle>();
        public List<Puzzle> ActivePuzzles => activePuzzles;
        public PuzzleController(List<Puzzle> puzzles)
        {
            activePuzzles = puzzles;
        }
    }
    private class LoreController
    {
        private List<Lore> activeLore = new List<Lore>();
        public List<Lore> ActiveLore => activeLore;
        public LoreController(List<Lore> lores)
        {
            activeLore = lores;
        }
    }
    private class ClueController
    {
        private List<BaseClue> foundClues = new List<BaseClue>();
        public List<BaseClue> FoundClues => foundClues;
        public ClueController(){ }
        public void AddClue(BaseClue clue)
        { 
            foundClues.Add(clue);
        }       
    }
}
