using Input;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
    [SerializeField] private GameObject menuInventory;
    [SerializeField] private GameObject craftTableUI;
    [SerializeField] private GameObject menuActive;
    public GameObject MenuInventory => menuInventory;
    public GameObject MenuActive => menuActive;
    private bool isPauseActive;
    public bool IsPauseActive => isPauseActive;

    public bool InventoryActive { get; private set; }
    public bool CraftTableActive { get; private set; }
    [Header("Day System")]
    [SerializeField] int _day;

    [Header("Timer System")]
    [SerializeField] float _time;
    public bool timerOn;
    public bool isTimeToSleep;
    public bool wentToSleep;
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

        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<audioManager>();

        objectivesText.text = "";
        daySystemText.text = "Day " + _day;
        timerText.text = _time.ToString();

        timerOn = true;
        isTimeToSleep = false;
        wentToSleep = false;

        timeScaleOG = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer(_time);       
    }

    public void PauseGame()
    {
        isPauseActive = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        audioManager.PauseSounds();
    }
    public void ActivatePauseMenu()
    {
        characterUI.SetActive(false);
        menuActive = pauseUI;
        menuActive.SetActive(true);
        audioManager.PlaySFX(audioManager.UIOpen, audioManager.UIVol);
    }
    public void UnpauseGame()
    {
        isPauseActive = false;
        Time.timeScale = timeScaleOG;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       
        audioManager.PauseSounds();
    }
    public void DeactivatePauseMenu()
    {
        menuActive.SetActive(false);
        menuActive = null;
        characterUI.SetActive(true);

        audioManager.PlaySFX(audioManager.UIClose, audioManager.UIVol);
    }
    public void ActivateInventoryUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuActive = menuInventory;
        menuActive.SetActive(true);
        InventoryActive = true;
    }
    public void DeactivateInventoryUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;       
        InventoryActive = false;
    }
    public void ActivateInventoryUISecondary()
    {
        menuInventory.SetActive(true);
        InventoryActive = true;
    }
    public void DeactivateInventoryUISecondary()
    {
        menuInventory.SetActive(false);
        InventoryActive = false;
    }
    public void ActivateCraftTableUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuActive = craftTableUI;
        menuActive.SetActive(true);
        CraftTableActive = true;
    }
    public void DeactivateCraftTableUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        CraftTableActive = false;
    }
    public void WinGame()
    {
        PauseGame();

        menuActive = winUI;
        menuActive.SetActive(true);

        audioManager.PlaySFX(audioManager.UIWin, audioManager.UIVol);
    }

    public void LoseGame()
    {
        PauseGame();
        menuActive = loseUI;
        menuActive.SetActive(true);

        audioManager.PlaySFX(audioManager.UILose, audioManager.UIVol);
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
  
}
