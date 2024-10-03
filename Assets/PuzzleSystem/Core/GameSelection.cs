using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSelection
{
    private Suspect killer;
    private MurderRoom room;
    private MurderWeapon weapon;
    private MurderMotive motive;
    private Case caseFile;
    public GameSelection(Suspect[] suspects, MurderRoom[] rooms, MurderWeapon[] weapons, Case[] cases, MurderMotive[] motives)
    {
        if(suspects.Length > 0)
        {
            killer = Randomizer.GetRandomizedObjectFromArray(suspects);
            killer.IsKiller = true;
        }
        if(rooms.Length > 0) 
        room = Randomizer.GetRandomizedObjectFromArray(rooms);
        if(weapons.Length > 0)
        weapon = Randomizer.GetRandomizedObjectFromArray(weapons);
        if(motives.Length > 0)
        motive = Randomizer.GetRandomizedObjectFromArray(motives);
        if(cases.Length > 0)
        caseFile = Randomizer.GetRandomizedObjectFromArray(cases);
    }
    public Suspect GetKiller() { return killer; }
    public MurderRoom GetRoom() { return room; }
    public MurderWeapon GetWeapon() { return weapon; }
    public MurderMotive GetMotive() { return motive; }
    public Case GetCase() { return caseFile; }
}
