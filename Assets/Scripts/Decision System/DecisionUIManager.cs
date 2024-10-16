using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecisionUIManager : MonoBehaviour
{
    [SerializeField] GameObject generateButton;
    private GameSelection gameSelection;
    [Header("Bindings")]
    private Dictionary<Toggle, BaseClueData> killerChoices = new Dictionary<Toggle, BaseClueData>();
    private Dictionary<Toggle, BaseClueData> motiveChoices = new Dictionary<Toggle, BaseClueData>();
    private Dictionary<Toggle, BaseClueData> weaponChoices = new Dictionary<Toggle, BaseClueData>();
    private Dictionary<Toggle, BaseClueData> roomChoices = new Dictionary<Toggle, BaseClueData>();
    [Header("Lists")]
    public List<Toggle> killerToggles = new List<Toggle>();
    public List<Toggle> motiveToggles = new List<Toggle>();
    public List<Toggle> weaponToggles = new List<Toggle>();
    public List<Toggle> roomToggles = new List<Toggle>();

    [SerializeField] BaseClueData selectedKiller;
    [SerializeField] BaseClueData selectedMotive;
    [SerializeField] BaseClueData selectedWeapon;
    [SerializeField] BaseClueData selectedRoom;
    void Awake()
    {
        killerToggles.ForEach(t => { t.isOn = false; killerChoices.Add(t, null); });
        motiveToggles.ForEach(t => { t.isOn = false; motiveChoices.Add(t, null); });
        weaponToggles.ForEach(t => { t.isOn = false; weaponChoices.Add(t, null); });
        roomToggles.ForEach(t => { t.isOn = false; roomChoices.Add(t, null); });
        generateButton.SetActive(false);
    }
    private void OnEnable()
    {
        EventSheet.SendAllClues += UpdateChoices;
        EventSheet.SendKillerClue += UpdateKiller;
        EventSheet.SendGameSelection += UpdateCorrectChoices;
    }
    private void OnDisable()
    {
        EventSheet.SendAllClues -= UpdateChoices;
        EventSheet.SendKillerClue -= UpdateKiller;
        EventSheet.SendGameSelection -= UpdateCorrectChoices;
    }
    private void Update()
    {
        ValidateSubmissionReady();
    }
    private void UpdateChoices(List<BaseClueData> clues)
    {
        foreach(BaseClueData clue in clues)
        {
            switch (clue)
            {
                case KillerClueData suspect:
                    AssignClueToEmptyToggle(killerChoices,suspect);
                    break;
                case MotiveClueData motive:
                    AssignClueToEmptyToggle(motiveChoices, motive);
                    break;
                case WeaponClueData weapon:
                    AssignClueToEmptyToggle(weaponChoices, weapon);
                    break;
                case RoomClueData room:
                    AssignClueToEmptyToggle(roomChoices, room);
                    break;
            }
        }
       
    }
    private void AssignClueToEmptyToggle(Dictionary<Toggle, BaseClueData> dict, BaseClueData data)
    {
        var keyWithNullValue = dict.FirstOrDefault(kvp => kvp.Value == null).Key;
        if(keyWithNullValue != null)
        {
            dict[keyWithNullValue] = data;
            UpdateToggleUI(keyWithNullValue,data);
        }
    }
    private void UpdateCorrectChoices(GameSelection selection)
    {
        gameSelection = selection;
    }   
    private void UpdateKiller(KillerClueData data)
    {
        AssignClueToEmptyToggle(killerChoices, data);
    }
    private void UpdateToggleUI(Toggle toggle, BaseClueData data)
    {
        TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
        if(text != null) 
            text.text = data.Name;
        toggle.image.sprite = data.Icon;
    }
    private void ValidateSubmissionReady()
    {
        if (selectedKiller != null && selectedRoom != null && selectedMotive != null && selectedWeapon != null)
        {
            generateButton.SetActive(true);
        }
        else
        {
            generateButton.SetActive(false);
            selectedKiller = killerChoices.Where(kvp => kvp.Key.isOn).Select(kvp => kvp.Value).FirstOrDefault();
            selectedWeapon = weaponChoices.Where(kvp => kvp.Key.isOn).Select(kvp => kvp.Value).FirstOrDefault();
            selectedRoom = roomChoices.Where(kvp => kvp.Key.isOn).Select(kvp => kvp.Value).FirstOrDefault();
            selectedMotive = motiveChoices.Where(kvp => kvp.Key.isOn).Select(kvp => kvp.Value).FirstOrDefault();
        }


    }
    public void MakeDecision()
    {
        Close();
        GameManager.instance.DeactivateCharacterUI();
        if(selectedKiller.ID == gameSelection.GetKiller().ID && selectedRoom.ID == gameSelection.GetRoom().ID && 
            selectedWeapon.ID == gameSelection.GetWeapon().ID && selectedMotive.ID == gameSelection.GetMotive().ID)
        {
            GameManager.instance.WinGame();
        }
        else
        {
            GameManager.instance.LoseGame();
        }

    }
    public void Close()
    {
        Input.InputManager.instance.EnableCharacterInputs();
        GameManager.instance.DeactivateDecisionUI();
    }
}
