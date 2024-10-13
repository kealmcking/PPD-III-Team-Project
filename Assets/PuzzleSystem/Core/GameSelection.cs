using System.Collections.Generic;
/// <summary>
/// Represents the randomly selected choices for each global objective. 
/// </summary>
public class GameSelection
{
    private SuspectData killer;
    private RoomClueData room;
    private WeaponClueData weapon;
    private MotiveClueData motive;
    private Case caseFile;
    public GameSelection(List<SuspectData> suspects, List<RoomClueData> rooms, List<WeaponClueData> weapons, List<Case> cases, List<MotiveClueData> motives)
    {
        if (suspects.Count > 0)
        {
            killer = Randomizer.GetRandomizedObjectFromList(suspects);
            killer.SuspectPrefab.GetComponent<Suspect>().IsKiller = true;
        }
        if (rooms.Count > 0)
            room = Randomizer.GetRandomizedObjectFromList(rooms);
        if (weapons.Count > 0)
            weapon = Randomizer.GetRandomizedObjectFromList(weapons);
        if (motives.Count > 0)
            motive = Randomizer.GetRandomizedObjectFromList(motives);
        if (cases.Count > 0)
        caseFile = Randomizer.GetRandomizedObjectFromList(cases);
    }
    public SuspectData GetKiller() { return killer; }
    public RoomClueData GetRoom() { return room; }
    public WeaponClueData GetWeapon() { return weapon; }
    public MotiveClueData GetMotive() { return motive; }
    public Case GetCase() { return caseFile; }
}
