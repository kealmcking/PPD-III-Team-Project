using Input;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;
    private audioManager audioManager;

  

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
    //[SerializeField] private GameObject menuInventory;
    [SerializeField] private GameObject menuActive;

    [Header("Day System")]
    [SerializeField] int _day;

    [Header("Timer System")]
    [SerializeField] float _time;

    [Header("Cogs")]
    [SerializeField] GameObject topCog;
    [SerializeField] GameObject leftCog;
    [SerializeField] GameObject rightCog;


    private float lerpSpeed = 0.2f;
    private Quaternion targetRotation;

    public bool timerOn;
    public bool isTimeToSleep;
    public bool wentToSleep;
    public bool isPaused;
    public bool isInventoryOpen;
    public int Day => _day;

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer(_time);
        if (InputManager.instance.getIsPause())
        {
            if (menuActive == null)
            {
                PauseGame();
                menuActive = pauseUI;
                menuActive.SetActive(isPaused);
            }
            else if(menuActive == pauseUI)
            {
                UnpauseGame();
            }
        }

        /*if (InputManager.instance.getIsInvOpen())
        {
            if (menuActive == null)
            {
                stateInventoryOpen();
                menuActive = menuInventory;
                menuInventory.SetActive(isInventoryOpen);
                rotateCogs();
            }
            else if (menuActive == menuInventory)
            {
                stateInventoryClose();
            }
        }*/
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        characterUI.SetActive(false);

        //audioManager.PlaySFX(audioManager.UIOpen, audioManager.UIVol);
    }

    public void UnpauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOG;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
        characterUI.SetActive(true);

        audioManager.PlaySFX(audioManager.UIClose, audioManager.UIVol);
    }

    /*public void stateInventoryOpen()
    {
        isInventoryOpen = !isInventoryOpen;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        characterUI.SetActive(false);
    }

    public void stateInventoryClose()
    {
        isInventoryOpen = !isInventoryOpen;
        Time.timeScale = timeScaleOG;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isInventoryOpen);
        menuActive = null;
        characterUI.SetActive(true);
    }*/

    public void WinGame()
    {
        PauseGame();
        menuActive = winUI;
        menuActive.SetActive(true);

        //audioManager.PlaySFX(audioManager.UIWin, audioManager.UIVol);
    }

    public void LoseGame()
    {
        PauseGame();
        menuActive = loseUI;
        menuActive.SetActive(true);

        //audioManager.PlaySFX(audioManager.UILose, audioManager.UIVol);
    }

    public void OptionsMenu()
    {
        optionsUI.SetActive(true);
        menuActive = optionsUI;
        pauseUI.SetActive(false);
        ButtonFunctions.instance.LoadOptions();

        audioManager.PlaySFX(audioManager.UIOpen, audioManager.UIVol);
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
            EventSheet.TodaysDayIndexIsThis.Invoke(_day);
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

    /*public void rotateCogs()
    {
        topCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        leftCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        rightCog.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }*/
}
