using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSelection
{
    private Suspect killer;
    private string roomName;
    private string weaponName;
    private Motive caseFile;
    public GameSelection(Suspect[] suspects, string[] rooms, string[] weapons, Motive[] motives)
    {
        killer = Randomizer.RandomizeArray(suspects);
        killer.IsKiller = true;
        roomName = Randomizer.RandomizeArray(rooms);
        weaponName = Randomizer.RandomizeArray(weapons);
        caseFile = Randomizer.RandomizeArray(motives);
    }
    public Suspect GetKiller() { return killer; }
    public string GetRoomName() { return roomName; }
    public string GetWeaponName() { return weaponName; }
    public Motive GetCase() { return caseFile; }
}
