using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSelection
{
    private Suspect killer;
    private MurderRoom room;
    private MurderWeapon weapon;
    private MurderMotive motive;
    private Case caseFile;
    public GameSelection(List<Suspect> suspects, List<MurderRoom> rooms, List<MurderWeapon> weapons, List<Case> cases, List<MurderMotive> motives)
    {
        if(suspects.Count > 0)
        {
            killer = Randomizer.GetRandomizedObjectFromList(suspects);
            killer.IsKiller = true;
        }
        if(rooms.Count > 0) 
        room = Randomizer.GetRandomizedObjectFromList(rooms);
        if(weapons.Count > 0)
        weapon = Randomizer.GetRandomizedObjectFromList(weapons);
        if(motives.Count > 0)
        motive = Randomizer.GetRandomizedObjectFromList(motives);
        if(cases.Count > 0)
        caseFile = Randomizer.GetRandomizedObjectFromList(cases);
    }
    public Suspect GetKiller() { return killer; }
    public MurderRoom GetRoom() { return room; }
    public MurderWeapon GetWeapon() { return weapon; }
    public MurderMotive GetMotive() { return motive; }
    public Case GetCase() { return caseFile; }
}
