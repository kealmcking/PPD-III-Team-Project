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
    [SerializeField] ClueInitializer initializer;
    [SerializeField, Tooltip("This contains the list of where the murder can take place. " +
        "It is also used to update the UI for instance when guessing the room in which the murder takes place")] List<RoomClueData> rooms;
    [SerializeField, Tooltip("This contains the list of the murder weapons. " +
        "It is also used to update the UI for instance when guessing the weapon which was used for the murder")] List<WeaponClueData> weapons;
    [SerializeField, Tooltip("This contains the list of the motives. " +
      "It is also used to update the UI for instance when guessing the motive which was used for the murder")] List<MotiveClueData> motives;
    [SerializeField, Tooltip("This contains the list of the motives. " +
     "It is also used to update the UI for instance when guessing the motive which was used for the murder")]
    List<KillerClueData> killers;
    [SerializeField, Tooltip("Add all potential suspects here")] List<SuspectData> suspectPool;
    [SerializeField, Tooltip("This represents all the possible cases available for this level")] List<Case> cases;

    [SerializeField, Tooltip("Represents the number of suspects that will be spawned for the game")] int suspectCount = 7;
    [SerializeField, Tooltip("Will be used as the ghost when a suspect dies")] GhostData ghost;

    public List<SuspectData> suspects;
    List<Puzzle> scenePuzzles;
    List<Lore> sceneLore;
    List<Puzzle> activePuzzles = new List<Puzzle>();
    private GameSelection gameSelection;
    public Suspect chosenKiller = null;
    public List<Suspect> sceneSuspects = new List<Suspect>();
    public List<Case> Cases { get { return cases; } private set { cases = value; } }
    public List<RoomClueData> Rooms { get { return rooms; } private set { rooms = value; } }
    public List<WeaponClueData> Weapons { get { return weapons; } private set { weapons = value; } }
    public List<MotiveClueData> Motives { get { return motives; } private set { motives = value; } }
    public GameSelection GameSelection => gameSelection;
    public List<BaseClueData> clues = new List<BaseClueData>();
    public void OnEnable()
    {
        EventSheet.SendAllClues += HandleAllClues;
        EventSheet.TodaysDayIndexIsThis += HandleDayChange;
        EventSheet.SendSceneSuspects += SetSceneSuspects;
    }
    public void OnDisable()
    {
        EventSheet.TodaysDayIndexIsThis -= HandleDayChange;
        EventSheet.SendSceneSuspects -= SetSceneSuspects;
        EventSheet.SendAllClues -= HandleAllClues;
    }
    public void Awake()
    {
        initializer ??= GetComponent<ClueInitializer>();
    }
    public void Start()
    {
        suspects = Randomizer.GetRandomizedGroupFromList(suspectPool, suspectCount);
        scenePuzzles = FindObjectsByType<Puzzle>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        sceneLore = FindObjectsByType<Lore>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        gameSelection = new GameSelection(suspects, killers, rooms, weapons, cases, motives);


        if (scenePuzzles.Count > 0)
        {
            foreach (Puzzle puzzle in gameSelection.GetCase().Puzzles)
            {
                foreach (var sPuzzle in scenePuzzles)
                {
                    if (puzzle.ID == sPuzzle.ID)
                    {
                        sPuzzle.GameObject().SetActive(true);
                        activePuzzles.Add(sPuzzle);
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
                    }
                }
            }

        }

        initializer.Initialize(killers, rooms, weapons, motives, activePuzzles);

        List<Suspect> newSuspects = suspects
            .Select(s => s.SuspectPrefab)
            .ToList();
        if (newSuspects.Count > 0)
        {
            EventSheet.InitializeSuspectsToScene?.Invoke(newSuspects, SpawnPointType.Starting, true);
            EventSheet.SendSuspects?.Invoke(suspects);
        }
        if (motives.Count > 0)
            EventSheet.SendMotives?.Invoke(motives);
        if (rooms.Count > 0)
            EventSheet.SendRooms?.Invoke(rooms);
        if (weapons.Count > 0)
            EventSheet.SendWeapons?.Invoke(weapons);
        if (gameSelection != null)
            EventSheet.SendGameSelection?.Invoke(gameSelection);
        
    }
    private void HandleDayChange(int day)
    {          
            Suspect suspect = Randomizer.GetRandomizedSuspectFromListAndRemove(ref sceneSuspects);
            Destroy(suspect.gameObject);
            EventSheet.SpawnGhost?.Invoke(ghost, SpawnPointType.Ghost, true, null);
            EventSheet.RelocateSuspects?.Invoke(sceneSuspects, SpawnPointType.Suspect, true);
        if (day >= 5)
        {          
            EventSheet.SpawnKiller?.Invoke(chosenKiller,SpawnPointType.Killer,true);
        }
    }
    private void HandleAllClues(List<BaseClueData> clues)
    {
        this.clues = clues; 
    }
    private void SetSceneSuspects(List<Suspect> scene)
    {
        sceneSuspects = scene;
        chosenKiller = sceneSuspects[UnityEngine.Random.Range(0, sceneSuspects.Count)];
        chosenKiller.IsKiller = true;
        KillerClueData clueToSend = null;
        foreach(var clue in clues)
        {
            if(clue is KillerClueData killerData)
            {
                if(killerData != null)
                {
                    if(killerData.data.ID == chosenKiller.Data.ID && killerData.data.Description == chosenKiller.Data.Description && killerData.data.Name == chosenKiller.Data.Name)
                    {
                        clueToSend = killerData; 
                    }
                }
            }
        }
        EventSheet.SendKillerClue?.Invoke(clueToSend);
    }
    private void SetChosenKiller(Suspect value)
    {
        chosenKiller = value;
    }
 

   
}
