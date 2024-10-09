using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;

public static class EventSheet 
{
    public static Action<List<Puzzle>> SendPuzzles;
    public static Action<List<Lore>> SendLore;
    public static Action<BaseItemData> SendFoundClue;
    public static Action<List<MurderRoom>> SendMurderRooms;
    public static Action<List<MurderWeapon>> SendMurderWeapons;
    public static Action<List<MurderMotive>> SendMurderMotives;
    public static Action<List<Suspect>> SendSuspects;
    public static Action<List<Suspect>, SpawnPointType, bool> InitializeSuspectsToScene;
    public static Action<GameSelection> SendGameSelection;
    public static Action<int> SendDay;
    public static Action NewDay;
    public static Action<Suspect> SuspectDied;
    public static Action<GhostData, SpawnPointType, bool,SpawnPoint> SpawnGhost;
    public static Action<Ghost> SendGhost;
    public static Action<int> TodaysDayIndexIsThis;
    public static Action<Suspect> SendKiller;
    public static Action<Suspect, SpawnPointType, bool> SpawnKiller;
}
