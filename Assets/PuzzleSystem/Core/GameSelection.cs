using System.Collections.Generic;
/// <summary>
/// Represents the randomly selected choices for each global objective. 
/// </summary>
public class GameSelection
{
    private KillerClueData killer;
    private RoomClueData room;
    private WeaponClueData weapon;
    private MotiveClueData motive;
    private Case caseFile;
    public GameSelection(List<SuspectData> suspects,List<KillerClueData> killers,List<RoomClueData> rooms, List<WeaponClueData> weapons, List<Case> cases, List<MotiveClueData> motives)
    {
        if (suspects.Count > 0)
        {
            
            for (int i = 0; i < suspects.Count; i++)
            {
                killers[i].SetName(suspects[i].Name);
                killers[i].SetDescription(suspects[i].Description);
                killers[i].SetIcon(suspects[i].Icon);
                killers[i].data = suspects[i];
            }
            killer = Randomizer.GetRandomizedObjectFromList(killers);
            killer.data.SuspectPrefab.IsKiller = true;
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
    public KillerClueData GetKiller() { return killer; }
    public RoomClueData GetRoom() { return room; }
    public WeaponClueData GetWeapon() { return weapon; }
    public MotiveClueData GetMotive() { return motive; }
    public Case GetCase() { return caseFile; }
    
}
