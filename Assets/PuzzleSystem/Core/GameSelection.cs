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
        killer = Randomizer.GetRandomizedObjectFromArray(suspects);
        killer.IsKiller = true;
        room = Randomizer.GetRandomizedObjectFromArray(rooms);
        weapon = Randomizer.GetRandomizedObjectFromArray(weapons);
        motive = Randomizer.GetRandomizedObjectFromArray(motives);
        caseFile = Randomizer.GetRandomizedObjectFromArray(cases);
    }
    public Suspect GetKiller() { return killer; }
    public MurderRoom GetRoom() { return room; }
    public MurderWeapon GetWeapon() { return weapon; }
    public MurderMotive GetMotive() { return motive; }
    public Case GetCase() { return caseFile; }
}
