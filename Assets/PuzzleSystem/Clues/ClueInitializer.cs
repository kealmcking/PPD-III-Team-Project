
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class ClueInitializer : MonoBehaviour
{
 
   // SuspectData chosenKiller = null;
    
    public void Initialize(GameSelection selection, List<KillerClueData> killers, List<RoomClueData> rooms, List<WeaponClueData> weapons, List<MotiveClueData> motives,List<Puzzle> activePuzzles)
    {
        List<BaseClueData> clues = new List<BaseClueData>();
       
        killers.ForEach(s =>
        {
           //  if (s.Name != selection.GetKiller().Name)
                clues.Add(s);
        });
       
        rooms.ForEach(s => 
            {
               // if (s.Name != selection.GetRoom().Name)
                clues.Add(s);
            });
        weapons.ForEach(s =>
        {
            //if (s.Name != selection.GetWeapon().Name)
                clues.Add(s);
        });
        motives.ForEach(s =>
        {
           // if (s.Name != selection.GetMotive().Name)
                clues.Add(s);
        });

        EventSheet.SendAllClues(clues);


        BaseClueData killer = null;
        BaseClueData room = null;
        BaseClueData weapon = null;
        BaseClueData motive = null;
        foreach (var clue in clues)
        {
            if(clue.Name == selection.GetKiller().Name)
            {
                killer = clue;
            }
            if( clue.Name == selection.GetRoom().Name)
            {
                room = clue;
            }
           if( clue.Name == selection.GetWeapon().Name)
            {
                weapon = clue;
            }
            if(clue.Name == selection.GetMotive().Name)
            {
                motive = clue;
            }                                   
        }
        clues.Remove(killer);
        clues.Remove(room);
        clues.Remove(weapon);
        clues.Remove(motive);

        for (int i = clues.Count - 1; i >= 0; i--)
        {
            if (clues[i] is MotiveClueData)
            {
                clues.RemoveAt(i);
            }
        }
        if (activePuzzles.Count > 0)
        {
            activePuzzles.ForEach(p =>
            {
                p.Reward = Randomizer.GetRandomizedObjectFromListAndRemove(ref clues);
            });
        }
        EventSheet.SpawnExcessClues?.Invoke(clues, SpawnPointType.Clue, true);

    }
}
