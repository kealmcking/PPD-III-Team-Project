
using System.Collections.Generic;
using System.Net;
using UnityEngine.AI;

public class ClueInitializer 
{
  
    List<SuspectData> suspects = new List<SuspectData>();
    List<RoomClueData> rooms = new List<RoomClueData>();
    List<WeaponClueData> weapons = new List<WeaponClueData>();
    List<MotiveClueData> motives = new List<MotiveClueData>();
    public ClueInitializer(List<SuspectData> suspects, List<RoomClueData> rooms, List<WeaponClueData> weapons, List<MotiveClueData> motives) 
    { 
        this.suspects = suspects;
        this.rooms = rooms;
        this.weapons = weapons;
        this.motives = motives;
    }
    public List<BaseClueData> Initialize(GameSelection selection)
    {
        List<BaseClueData> clues = new List<BaseClueData>();
        foreach (var suspect in suspects) {
            //if (suspect.ID == selection.GetKiller().ID) continue;
            
            KillerClueData data = (KillerClueData)KillerClueData.CreateInstance("KillerClueData");
            if(data.Icon != null)
               data.SetIcon(suspect.Icon);
            if(data.Suspect != null)
               data.SetSuspectData(suspect);
            if(data.Name != null)
               data.SetName(suspect.Name);
            if(data.Description != null)
               data.SetDescription(suspect.Description);
            if (suspect.ID == selection.GetKiller().ID)
            {
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
