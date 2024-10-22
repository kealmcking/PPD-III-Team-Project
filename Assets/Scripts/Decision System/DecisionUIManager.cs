using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecisionUIManager : MonoBehaviour
{
    [SerializeField] GameObject generateButton;
    private GameSelection gameSelection;
    [Header("Bindings")]
    private List<KillerClueData> killerChoices = new List<KillerClueData>();
    private List<MotiveClueData> motiveChoices = new List<MotiveClueData>();
    private List<WeaponClueData> weaponChoices = new List<WeaponClueData>();
    private List<RoomClueData> roomChoices = new List<RoomClueData>();
    [Header("Lists")]
    public List<Toggle> killerToggles = new List<Toggle>();
    public List<Toggle> motiveToggles = new List<Toggle>();
    public List<Toggle> weaponToggles = new List<Toggle>();
    public List<Toggle> roomToggles = new List<Toggle>();

    [SerializeField] public KillerClueData selectedKiller;
    [SerializeField] public MotiveClueData selectedMotive;
    [SerializeField] public WeaponClueData selectedWeapon;
    [SerializeField] public RoomClueData selectedRoom;
    public KillerClueData killer;
    public MotiveClueData motive;
    public WeaponClueData weapon;
    public RoomClueData room;
    void Awake()
    {
        killerToggles.ForEach(t => { t.isOn = false;});
        motiveToggles.ForEach(t => { t.isOn = false;});
        weaponToggles.ForEach(t => { t.isOn = false;});
        roomToggles.ForEach(t => { t.isOn = false;});
        generateButton.SetActive(false);
    }
    private void OnEnable()
    {
        EventSheet.SendAllClues += UpdateChoices;
        EventSheet.SendGameSelection += UpdateCorrectChoices;
    }
    private void OnDisable()
    {
        EventSheet.SendAllClues -= UpdateChoices;
        EventSheet.SendGameSelection -= UpdateCorrectChoices;
    }
    private void Update()
    {
        ValidateSubmissionReady();
    }
    public void UpdateKillerSelection(int index)
    {
        if (index < 0
            || index > killerChoices.Count
            || index == killerChoices.IndexOf(selectedKiller))
            return;

        Debug.Log("obj" + index + "selected");
        selectedKiller = null;
        killerToggles[index].isOn = false;

        selectedKiller = killerChoices[index];
        killerToggles.ElementAt(index).isOn = true;

    }
    public void UpdateMotiveSelection(int index)
    {
        if (index < 0
        || index > motiveChoices.Count
        || index == motiveChoices.IndexOf(selectedMotive))
            return;

        Debug.Log("obj" + index + "selected");
        selectedMotive = null;
       motiveToggles[index].isOn = false;

        selectedMotive = motiveChoices[index];
        motiveToggles.ElementAt(index).isOn = true;

    }
    public void UpdateWeaponSelection(int index)
    {
        if (index < 0
         || index > weaponChoices.Count
         || index == weaponChoices.IndexOf(selectedWeapon))
            return;

        Debug.Log("obj" + index + "selected");
        selectedWeapon = null;
        weaponToggles[index].isOn = false;

        selectedWeapon = weaponChoices[index];
        weaponToggles.ElementAt(index).isOn = true;

    }
    public void UpdateRoomSelection(int index)
    {
        if (index < 0
           || index > roomChoices.Count
           || index == roomChoices.IndexOf(selectedRoom))
            return;

        Debug.Log("obj" + index + "selected");
        selectedRoom = null;
        roomToggles[index].isOn = false;

        selectedRoom = roomChoices[index];
        roomToggles.ElementAt(index).isOn = true;

    }
    private void UpdateChoices(List<BaseClueData> clues)
    {
        
        foreach (BaseClueData clue in clues)
        {
            switch (clue)
            {
                case KillerClueData suspect:
                    killerChoices.Add(suspect);
                    UpdateToggleUI(killerToggles.ElementAt(killerChoices.IndexOf(suspect)), suspect);
                    break;
                case MotiveClueData motive:
                    motiveChoices.Add(motive);
                    UpdateToggleUI(motiveToggles.ElementAt(motiveChoices.IndexOf(motive)), motive);
                    break;
                case WeaponClueData weapon:
                    weaponChoices.Add(weapon);
                    UpdateToggleUI(weaponToggles.ElementAt(weaponChoices.IndexOf(weapon)), weapon);
                    break;
                case RoomClueData room:
                    roomChoices.Add(room);
                    UpdateToggleUI(roomToggles.ElementAt(roomChoices.IndexOf(room)), room);
                    break;
            }
        }
       
    }
    private void UpdateCorrectChoices(GameSelection selection)
    {
        gameSelection = selection;
      
        motive = gameSelection.GetMotive();
        weapon = gameSelection.GetWeapon();
        room = gameSelection.GetRoom();
        killer = gameSelection.GetKiller();
    }   
  
    private void UpdateToggleUI(Toggle toggle, BaseClueData data)
    {
        TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
        if(text != null) 
            text.text = data.Name;
        if(data.Icon != null)
        toggle.image.sprite = data.Icon;
    }
    private void ValidateSubmissionReady()
    {
        if (selectedKiller != null && selectedRoom != null && selectedMotive != null && selectedWeapon != null )
        {
            generateButton.SetActive(true);
        }
        else 
        {
          //  generateButton.SetActive(false);
           
        }


    }
    private bool ValidateKiller()
    {
        if (selectedKiller.data.ID == gameSelection.GetKiller().ID && selectedKiller.data.Description == gameSelection.GetKiller().Description && selectedKiller.data.Name == gameSelection.GetKiller().Name)
        return true;
        else return false;
    }
    private bool ValidateMotive()
    {
        if (selectedMotive.ID == gameSelection.GetMotive().ID && selectedMotive.Description == gameSelection.GetMotive().Description && selectedMotive.Name == gameSelection.GetMotive().Name)
            return true;
        else return false;
    }
    private bool ValidateWeapon()
    {
        if (selectedWeapon.ID == gameSelection.GetWeapon().ID && selectedWeapon.Description == gameSelection.GetWeapon().Description && selectedWeapon.Name == gameSelection.GetWeapon().Name)
            return true;
        else return false;
    }
    private bool ValidateRoom()
    {
        if (selectedRoom.ID == gameSelection.GetRoom().ID && selectedRoom.Description == gameSelection.GetRoom().Description && selectedRoom.Name == gameSelection.GetRoom().Name)
            return true;
        else return false;
    }
    public void MakeDecision()
    {
        Debug.Log("Killer" + gameSelection.GetKiller().Name + "Motive" + gameSelection.GetMotive().Name + "Room" + gameSelection.GetRoom().Name + "Weapon" + gameSelection.GetWeapon().Name);
        Debug.Log("SelectedKiller" + selectedKiller.Name + "SelectedMotive" + selectedMotive.Name + "SelectedRoom" + selectedRoom.Name + "SelectedWeapon" + selectedWeapon.Name);
      
        GameManager.instance.DeactivateCharacterUI();
        if(selectedRoom.Name == gameSelection.GetRoom().Name && selectedWeapon.Name == gameSelection.GetWeapon().Name && selectedMotive.Name == gameSelection.GetMotive().Name && selectedKiller.Name == gameSelection.GetKiller().Name)
        {
            Close();
            GameManager.instance.WinGame();
        }
        else
        {
            Close();
            GameManager.instance.LoseGame();
        }
        
    }
    public void Close()
    {
        Input.InputManager.instance.EnableCharacterInputs();
        GameManager.instance.DeactivateDecisionUI();
    }
}
