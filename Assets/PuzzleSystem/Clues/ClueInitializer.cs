
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class ClueInitializer : MonoBehaviour
{
 
    SuspectData chosenKiller = null;
    
    public List<BaseClueData> Initialize(GameSelection selection, List<SuspectData> suspects, List<RoomClueData> rooms, List<WeaponClueData> weapons, List<MotiveClueData> motives)
    {
        List<BaseClueData> clues = new List<BaseClueData>();
        foreach (var suspect in suspects) {
            //if (suspect.ID == selection.GetKiller().ID) continue;
            Debug.Log("Suspect: " + suspect.name);
            KillerClueData data = ScriptableObject.CreateInstance<KillerClueData>();
            if(data.Icon != null)
               data.SetIcon(suspect.Icon);
            if(data.Suspect != null)
               data.SetSuspectData(suspect);
            if(data.Name != null)
               data.SetName(suspect.Name);
            if(data.Description != null)
               data.SetDescription(suspect.Description);
            if (chosenKiller == null)
            {                
                data.Suspect.SuspectPrefab.GetComponent<Suspect>().IsKiller = true;
                chosenKiller = suspect;
                EventSheet.SendKillerClue?.Invoke(data);
            }
            else
                clues.Add(data);
        }
        rooms.ForEach(s => 
            {
                if (s.ID != selection.GetRoom().ID)
                clues.Add(s);
            });
        weapons.ForEach(s =>
        {
            if (s.ID != selection.GetWeapon().ID)
                clues.Add(s);
        });
        motives.ForEach(s =>
        {
            if (s.ID != selection.GetMotive().ID)
                clues.Add(s);
        });
        EventSheet.SendAllClues(clues);
        selection.GetCase().Puzzles.ForEach(p =>
        {
            p.Reward = Randomizer.GetRandomizedObjectFromListAndRemove(ref clues);
        });
      return clues;
    }
}
