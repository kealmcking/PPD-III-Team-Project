using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;

public static class EventSheet 
{
    public static Action<List<Puzzle>> SendPuzzles;
    public static Action<List<Lore>> SendLore;
    public static Action<BaseClueData> SendFoundClue;
    public static Action<List<RoomClueData>> SendRooms;
    public static Action<List<WeaponClueData>> SendWeapons;
    public static Action<List<MotiveClueData>> SendMotives;
    public static Action<List<SuspectData>> SendSuspects;
    public static Action<Suspect,List<Suspect>, SpawnPointType, bool> InitializeSuspectsToScene;
    public static Action<GameSelection> SendGameSelection;
    public static Action<int> SendDay;
    public static Action NewDay;
    public static Action<Suspect> SuspectDied;
    public static Action<GhostData, SpawnPointType, bool,SpawnPoint> SpawnGhost;
    public static Action<Ghost> SendGhost;
    public static Action<int> TodaysDayIndexIsThis;
    public static Action<Suspect, SpawnPointType, bool> SpawnKiller;
    public static Action<List<BaseClueData>, SpawnPointType,bool> SpawnExcessClues;
    public static Action<Item> SendItemToInventory;
    public static Action<Clue> SendClueToTracker;
    public static Action IHavePressedInteractButton;
    public static Action<KillerClueData> SendKillerClue;
    public static Action<List<BaseClueData>> SendAllClues;
    public static Action<SuspectData> SendKillerData;
    public static Action<List<Suspect>> SendSceneSuspects;
    public static Action<List<Suspect>, SpawnPointType, bool> RelocateSuspects;
    public static Action<BaseItemData> EquipItem;
    public static Action UseHeldItem;
    public static Action DropHeldItem;
    public static Action ThrowHeldItem;
    public static Action ThrowAnimationEvent;
    public static Action ItemColliderAnimationEvent;
    public static Action<bool> GateConditionStatus;
}
