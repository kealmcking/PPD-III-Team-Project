
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class ClueInitializer : MonoBehaviour
{
 
   // SuspectData chosenKiller = null;
    
    public void Initialize(GameSelection selection, List<KillerClueData> killers, List<SuspectData> suspects, List<RoomClueData> rooms, List<WeaponClueData> weapons, List<MotiveClueData> motives)
    {
        List<BaseClueData> clues = new List<BaseClueData>();
       
        killers.ForEach(s =>
        {
           // if (s.ID != selection.GetKiller().ID)
                clues.Add(s);
        });
       
        rooms.ForEach(s => 
            {
               // if (s.ID != selection.GetRoom().ID)
                clues.Add(s);
            });
        weapons.ForEach(s =>
        {
           // if (s.ID != selection.GetWeapon().ID)
                clues.Add(s);
        });
        motives.ForEach(s =>
        {
           // if (s.ID != selection.GetMotive().ID)
                clues.Add(s);
        });

        EventSheet.SendAllClues(clues);
        if(selection.GetCase().Puzzles.Count > 0)
        {
            selection.GetCase().Puzzles.ForEach(p =>
            {
                p.Reward = Randomizer.GetRandomizedObjectFromListAndRemove(ref clues);
            });
        }
        EventSheet.SpawnExcessClues?.Invoke(clues, SpawnPointType.Clue, true);

    }
}
