using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;


public class SpawnManager : MonoBehaviour 
{
    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();  
    private void Awake()
    {
       spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList(); 

    } 
    private void OnEnable()
    {
        EventSheet.InitializeSuspectsToScene += SpawnGroupByMono;
        EventSheet.SpawnGhost += SpawnGhost;
        EventSheet.SpawnKiller += SpawnKiller;
        EventSheet.SpawnExcessClues += SpawnClues;
        EventSheet.RelocateSuspects += RelocateRemainingSuspects;
    }
    private void OnDisable()
    {
        EventSheet.InitializeSuspectsToScene -= SpawnGroupByMono;
        EventSheet.SpawnGhost -= SpawnGhost;
        EventSheet.SpawnKiller -= SpawnKiller;
        EventSheet.SpawnExcessClues -= SpawnClues;
        EventSheet.RelocateSuspects -= RelocateRemainingSuspects;
    }
    private void SpawnGhost(GhostData ghost, SpawnPointType type, bool randomize = false, SpawnPoint spawnPoint = null) 
    {
        if (randomize)
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
                SpawnPoint spawn = Randomizer.GetRandomizedObjectFromList(filteredSpawns);
            Ghost instance = Instantiate(ghost.Prefab);
            instance.transform.position = spawn.transform.position;
           // EventSheet.SendGhost?.Invoke(instance);

        }
        else if(spawnPoint != null)
        {
            Ghost instance = Instantiate(ghost.Prefab);
            instance.gameObject.transform.position = spawnPoint.transform.position;
            EventSheet.SendGhost?.Invoke(instance);
        }
        else
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
               .Where(s => s.Type == type)
               .ToList();


            SpawnPoint spawn = filteredSpawns.FirstOrDefault();
            Ghost instance = Instantiate(ghost.Prefab);
            instance.gameObject.transform.position = spawn.transform.position;
            EventSheet.SendGhost?.Invoke(instance);
        }
    }
    private void SpawnObjectByMono<T>(T obj, SpawnPointType type, SpawnPoint spawningPoint) where T : MonoBehaviour
    {           
            Instantiate(obj).gameObject.transform.position = spawningPoint.transform.position;
    }
    private void RelocateRemainingSuspects(List<Suspect> group, SpawnPointType type, bool randomize = false)
    {
        if (randomize)
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
            foreach (var item in group)
            {
                SpawnPoint spawn = Randomizer.GetRandomizedObjectFromListAndRemove(ref filteredSpawns);
                item.GetComponent<NavMeshAgent>().enabled = false;
                item.transform.position = spawn.transform.position;
                item.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
        else
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();

            for (int i = 0; i < group.Count; i++)
            {
                SpawnPoint spawn = filteredSpawns.ElementAt(i);
                group.ElementAt(i).transform.position = spawn.transform.position;
            }
        }
    }
    private void SpawnGroupByMono(List<Suspect> group, SpawnPointType type, bool randomize = false) 
    {
        if (randomize)
        {
            List<Suspect> sus = new List<Suspect>();
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
            foreach(var item in group)
            {
                SpawnPoint spawn = Randomizer.GetRandomizedObjectFromListAndRemove(ref filteredSpawns);
                Suspect sceneSuspect = Instantiate(item);
                sceneSuspect.transform.position = spawn.transform.position;
                sus.Add(sceneSuspect);

            }
            EventSheet.SendSceneSuspects?.Invoke(sus);
        }
        else
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
          
            for(int i = 0; i < group.Count; i++)
            {
                SpawnPoint spawn = filteredSpawns.ElementAt(i);
              //  Instantiate(group.ElementAt(i)).gameObject.transform.position = spawn.transform.position;
            }
        }
    }
    private void SpawnGroupByObject(List<GameObject> group, SpawnPointType type, bool randomize = false) 
    {
        if (randomize)
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
            foreach (var item in group)
            {
                SpawnPoint spawn = Randomizer.GetRandomizedObjectFromListAndRemove(ref filteredSpawns);
                Instantiate(item).gameObject.transform.position = spawn.transform.position;
            }
        }
        else
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();

            for (int i = 0; i < group.Count; i++)
            {
                SpawnPoint spawn = filteredSpawns.ElementAt(i);
                Instantiate(group.ElementAt(i)).gameObject.transform.position = spawn.transform.position;
            }
        }
    }
    private void SpawnKiller(Suspect killer, SpawnPointType type, bool randomize = false)
    {
        if (randomize)
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
            SpawnPoint spawn = Randomizer.GetRandomizedObjectFromList(filteredSpawns);
           // killer.ActivateMask();
           killer.GetComponent<NavMeshAgent>().enabled = false;
            killer.transform.position = spawn.transform.position;
            killer.GetComponent<NavMeshAgent>().enabled = true;

        }
        else
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
               .Where(s => s.Type == type)
               .ToList();


            SpawnPoint spawn = filteredSpawns.FirstOrDefault();
           //killer.ActivateMask();
            killer.gameObject.transform.position = spawn.transform.position;
          
        }
    }
    private void SpawnClues(List<BaseClueData> clues, SpawnPointType type, bool randomize = false)
    {
       
        if (randomize)
        {
           
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();
            foreach (BaseClueData item in clues)
            {
               
                SpawnPoint spawn = Randomizer.GetRandomizedObjectFromList(filteredSpawns);
                Clue prefab = Instantiate(item.Prefab);
               
                prefab.gameObject.transform.position = spawn.transform.position;
            }
        }
        else
        {
            List<SpawnPoint> filteredSpawns = spawnPoints
                .Where(s => s.Type == type)
                .ToList();

            for (int i = 0; i < clues.Count; i++)
            {
                SpawnPoint spawn = filteredSpawns.ElementAt(i);
                Instantiate(clues.ElementAt(i).Prefab).gameObject.transform.position = spawn.transform.position;
            }
        }
    }
}
