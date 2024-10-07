using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private TextMeshProUGUI daySystemText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Menu UI")]
    [SerializeField] private GameObject sleepUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject menuActive;

    [Header("Input")]
    private InputActionMap playerInputActions;

    [Header("Day System")]
    [SerializeField] int _day;

    [Header("Timer System")]
    [SerializeField] float _time;


    [Header("Tracker Toggles")]
    [SerializeField] private List<Toggle> suspectList;
    [SerializeField] private List<Toggle> weaponList;
    [SerializeField] private List<Toggle> roomList;
    [SerializeField] private List<Toggle> motiveList;


    [SerializeField] Button resumeOptionsButtons;
    [SerializeField] Button resumePauseButtons;
    [SerializeField] Button replayLoseButtons;
    [SerializeField] Button replayWinButtons;
    [SerializeField] Button quitWinButtons;
    [SerializeField] Button quitLoseButtons;
    [SerializeField] Button quitPauseButtons;
    [SerializeField] Button optionsButton;

    private GameSelection gameSelection;
    

    public bool timerOn;
    public bool isTimeToSleep;
    public bool wentToSleep;
    public bool isPaused;

    private float DayLength;
    [Range(1, 60), SerializeField, Tooltip("The length each day (round) should be. The number placed here is multiplied by 60 to represent one minute " +
        "Example: 5 = 5 minutes")]
    int dayLength;
    private float currentTimer = 0;
    public int DayCounter
    {
        get { return DayCounter; }
        private set
        {
            DayCounter = value;
            //SendDayCounter.Invoke(DayCounter);
            UpdateDayText(DayCounter);
            if (DayCounter <= 1)
            {
                FinalDay.Invoke(gameSelection.GetKiller());
            }
        }
    }
    private bool isTimerGoing = false;
    public static Action<Suspect> FinalDay;

    float timeScaleOG;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else return;

        objectivesText.text = "";
        daySystemText.text = "Day " + _day;
        timerText.text = _time.ToString();

        timerOn = true;
        isTimeToSleep = false;
        wentToSleep = false;
        isPaused = false;

        timeScaleOG = Time.timeScale;

        suspectList = new List<Toggle>();
        weaponList = new List<Toggle>();
        roomList = new List<Toggle>();
        motiveList = new List<Toggle>();

        optionsButton.AddComponent<Selectable>();
        resumeOptionsButtons.AddComponent<Selectable>();
        resumePauseButtons.AddComponent<Selectable>();
        quitWinButtons.AddComponent<Selectable>();
        quitLoseButtons.AddComponent<Selectable>();
        quitPauseButtons.AddComponent<Selectable>();

    }

    private void OnEnable()
    {
        //Director.SendMurderRooms += UpdateMurderRoom;
        //Director.SendMurderWeapons += UpdateMurderWeapons;
        //Director.SendSuspects += UpdateSuspects;
        //Director.SendMurderMotives += UpdateMotives;
        //Director.SendGameSelection += UpdateGameSelection;
        //Director.SendFoundClue += UpdateToggles;
       //StartTimer();
    }

    private void OnDisable()
    {
        //Director.SendMurderRooms -= UpdateMurderRoom;
        //Director.SendMurderWeapons -= UpdateMurderWeapons;
        //Director.SendSuspects -= UpdateSuspects;
        //Director.SendMurderMotives -= UpdateMotives;
        //Director.SendGameSelection -= UpdateGameSelection;
        //Director.SendFoundClue -= UpdateToggles;
    }

    // Update is called once per frame
    void Update()
    {
         UpdateTimer(_time);
    }

    public void UpdateToggles(BaseClueData clue)
    {
        foreach (Toggle toggle in weaponList)
        {
            if(toggle.name == clue.name)
            {
                toggle.isOn = true;
            }
        }
        foreach (Toggle toggle in suspectList)
        {
            if (toggle.name == clue.name)
            {
                toggle.isOn = true;
            }
        }
        foreach (Toggle toggle in roomList)
        {
            if (toggle.name == clue.name)
            {
                toggle.isOn = true;
            }
        }
        foreach (Toggle toggle in motiveList)
        {
            if (toggle.name == clue.name)
            {
                toggle.isOn = true;
            }
        }
    }

    public void UpdateGameSelection(GameSelection selection)
    {
        gameSelection = selection;

    }

    public void UpdateSuspects(List<Suspect> suspects)
    {
        foreach(Toggle toggle in suspectList)
        {
            foreach(Suspect suspect in suspects)
            {
                toggle.GetComponentInChildren<Toggle>().image.sprite = suspect.Icon;
                toggle.name = suspect.Name;
            }
        }
    }

    public void UpdateMurderWeapons(List<MurderWeapon> murderweapons)
    {
        foreach (Toggle toggle in weaponList)
        {
            foreach (MurderWeapon weapon in murderweapons)
            {
                toggle.GetComponentInChildren<Toggle>().image.sprite = weapon.Icon;
                toggle.name = weapon.Name;
            }
        }
    }

    public void UpdateMurderRoom(List<MurderRoom> murderRooms)
    {
        foreach (Toggle toggle in roomList)
        {
            foreach (MurderRoom room in murderRooms)
            {
                toggle.GetComponentInChildren<Toggle>().image.sprite = room.Icon;
                toggle.name = room.Name;
                
            }
        }
    }

    public void UpdateMotives(List<MurderMotive> motives)
    {
        foreach (Toggle toggle in motiveList)
        {
            foreach (MurderMotive motive in motives)
            {
                toggle.GetComponentInChildren<Toggle>().image.sprite = motive.Icon;
                toggle.name= motive.Name;  
            }
        }
    }

    

    public void PauseGame()
    {
        
        EventSystem.current.SetSelectedGameObject(resumePauseButtons.gameObject);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        characterUI.SetActive(false);
        menuActive = pauseUI;
        menuActive.SetActive(true);
    }

    public void UnpauseGame()
    {
       
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
        characterUI.SetActive(true);
    }

    public void WinGame()
    {
        EventSystem.current.SetSelectedGameObject(replayWinButtons.gameObject);
        PauseGame();
        menuActive = winUI;
        menuActive.SetActive(true);
    }

    public void LoseGame()
    {
        EventSystem.current.SetSelectedGameObject(replayLoseButtons.gameObject);
        PauseGame();
        menuActive = loseUI;
        menuActive.SetActive(true);
    }

    public void OptionsMenu()
    {
        optionsUI.SetActive(true);
        menuActive = optionsUI;
        pauseUI.SetActive(false);
       
    }

    public void UpdateObjectiveText(string text)
    {
        objectivesText.text = text;
    }
    public void UpdateDayText(int day)
    {
        daySystemText.text = "Day " + day.ToString();
    }
    public void UpdateTimer(float time)
    {
        _time = time;
        if (timerOn && _time > 0)
        {
            
            _time -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(_time / 60);
            int seconds = Mathf.FloorToInt(_time % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            _time = 0;
            timerOn = false;
            timerText.text = "Go To Sleep";
            isTimeToSleep = true;
        }
        
    }
    
  

    private void TimeToGoToSleep()
    {
        if(isTimeToSleep && wentToSleep)
        {
            
            _day++;
            UpdateDayText(_day);
            StartCoroutine(Sleeping());
            if (_day == 7)
            {
                FinalDay.Invoke(gameSelection.GetKiller());
            }
        }
    }

    IEnumerator Sleeping()
    {
        sleepUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        sleepUI.SetActive(false);
    }
    
}
