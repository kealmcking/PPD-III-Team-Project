using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSelection
{
    private Suspect killer;
    private Room room;
    private MurderWeapon weapon;
    private Motive caseFile;
    public GameSelection(Suspect[] suspects, Room[] rooms, MurderWeapon[] weapons, Motive[] motives)
    {
        killer = Randomizer.GetRandomizedObjectFromArray(suspects);
        killer.IsKiller = true;
        room = Randomizer.GetRandomizedObjectFromArray(rooms);
        weapon = Randomizer.GetRandomizedObjectFromArray(weapons);
        caseFile = Randomizer.GetRandomizedObjectFromArray(motives);
    }
    public Suspect GetKiller() { return killer; }
    public Room GetRoomName() { return room; }
    public MurderWeapon GetWeaponName() { return weapon; }
    public Motive GetCase() { return caseFile; }
}
