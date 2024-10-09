using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the puzzle system and determines the initial game state.
/// </summary>
public class Director : MonoBehaviour
{

    [SerializeField,Tooltip("This contains the list of where the murder can take place. " +
        "It is also used to update the UI for instance when guessing the room in which the murder takes place")] List<MurderRoom> rooms;
    [SerializeField,Tooltip("This contains the list of the murder weapons. " +
        "It is also used to update the UI for instance when guessing the weapon which was used for the murder")] List<MurderWeapon> weapons;
    [SerializeField, Tooltip("This contains the list of the motives. " +
      "It is also used to update the UI for instance when guessing the motive which was used for the murder")] List<MurderMotive> motives;
    [SerializeField,Tooltip("This represents all the possible cases available for this level")] List<Case> cases;
    [SerializeField, Tooltip("Add all potential suspects here")] List<SuspectData> suspectPool;
    [SerializeField, Tooltip("Represents the number of suspects that will be spawned for the game")] int suspectCount = 7;
    [SerializeField,Tooltip("Will be used as the ghost when a suspect dies")] GhostData ghost;
    List<SuspectData> suspects;
    List<Puzzle> scenePuzzles;
    List<Lore> sceneLore;
    
    private GameSelection gameSelection;
    private ClueController cController;
    private PuzzleController pController;
    private LoreController lController;
    
    public List<Case> Cases { get { return cases; } private set { cases = value; } }
    public List<MurderRoom> Rooms { get { return rooms; } private set { rooms = value; } }
    public List<MurderWeapon> Weapons { get { return weapons; } private set { weapons = value; } }
    public List<MurderMotive> Motives { get { return motives; } private set { motives = value; } }
    public GameSelection GameSelection => gameSelection;
    public void OnEnable()
    {
        EventSheet.TodaysDayIndexIsThis += HandleDayChange;
    }
    public void OnDisable()
    {
        EventSheet.TodaysDayIndexIsThis -= HandleDayChange;
    }
    public void Start()
    {
        suspects = Randomizer.GetRandomizedGroupFromList(suspectPool, suspectCount);
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
                foreach(var sPuzzle in scenePuzzles)
                {
                    if (puzzle.ID == sPuzzle.ID)
                    {
                        sPuzzle.GameObject().SetActive(true);
                        activeP.Add(sPuzzle);
                    }
                }
               
               
            }
        }
        if (sceneLore.Count > 0)
        {
            foreach (Lore lore in gameSelection.GetCase().Lore)
            {
                foreach (var sLore in sceneLore)
                {
                    if (lore.ID == sLore.ID)
                    {
                        sLore.GameObject().SetActive(true);
                        activeL.Add(sLore);
                    }
                }
            }
           
        }
        pController = new PuzzleController(activeP);
        lController = new LoreController(activeL);

        List<Suspect> newSuspects = suspects
            .Select(s => s.SuspectPrefab)
            .ToList();
        if(newSuspects.Count > 0)
        {
            EventSheet.InitializeSuspectsToScene?.Invoke(newSuspects, SpawnPointType.Starting, true);
            EventSheet.SendSuspects?.Invoke(newSuspects);
        }
        if(motives.Count > 0)
            EventSheet.SendMurderMotives?.Invoke(motives);
        if (rooms.Count > 0)
            EventSheet.SendMurderRooms?.Invoke(rooms);
        if (weapons.Count > 0)
            EventSheet.SendMurderWeapons?.Invoke(weapons);
        if (gameSelection != null)
            EventSheet.SendGameSelection?.Invoke(gameSelection);
    }
    private void HandleDayChange(int day)
    {
        if (day < suspects.Count - 1)
        {
            List<Suspect> newSuspects = suspects
                .Select(s => s.SuspectPrefab)
                .ToList();
            Suspect suspect = Randomizer.GetConditionalRandomizedSuspectFromListAndRemove(ref newSuspects);
            EventSheet.SuspectDied?.Invoke(suspect);
            ghost.Prefab.SuspectData = suspect.Data;
            EventSheet.SpawnGhost?.Invoke(ghost, SpawnPointType.Ghost, true, null);
        }
        else
        {
            List<Suspect> newSuspects = suspects
              .Select(s => s.SuspectPrefab)
              .ToList();
            Suspect suspect = Randomizer.GetConditionalRandomizedSuspectFromListAndRemove(ref newSuspects);
            EventSheet.SpawnKiller?.Invoke(suspect,SpawnPointType.Killer,true);
        }
    }
    private class PuzzleController
    {
        private List<Puzzle> activePuzzles = new List<Puzzle>();
        public List<Puzzle> ActivePuzzles => activePuzzles;
        public PuzzleController(List<Puzzle> puzzles)
        {
            activePuzzles = puzzles;
            EventSheet.SendPuzzles?.Invoke(activePuzzles);
        }
    }
    private class LoreController
    {
        private List<Lore> activeLore = new List<Lore>();
        public List<Lore> ActiveLore => activeLore;
        public LoreController(List<Lore> lores)
        {
            activeLore = lores;
            EventSheet.SendLore?.Invoke(activeLore);
        }
    }
    private class ClueController
    {
        private List<BaseItemData> foundClues = new List<BaseItemData>();
        public ClueController(){ }
        public void AddClue(BaseItemData clue)
        { 
            foundClues.Add(clue);
            EventSheet.SendFoundClue?.Invoke(clue);
        }       
    }
}
