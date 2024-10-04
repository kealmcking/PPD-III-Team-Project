using Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private List<bool> suspectListbools;


    public bool timerOn;
    public bool isTimeToSleep;
    public bool wentToSleep;
    public bool isPaused;

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer(_time);
        
    }

    public void TrackerMenu()
    {
        
    }

    public void PauseGame()
    {
        
        
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
        PauseGame();
        menuActive = winUI;
        menuActive.SetActive(true);
    }

    public void LoseGame()
    {
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

            float minutes = Mathf.FloorToInt(_time / 60);
            float seconds = Mathf.FloorToInt(_time % 60);

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
        }
    }

    IEnumerator Sleeping()
    {
        sleepUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        sleepUI.SetActive(false);
    }
}
