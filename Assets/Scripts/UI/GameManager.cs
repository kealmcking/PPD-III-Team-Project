using Input;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   // private audioManager audioManager;


    [SerializeField] GameObject player;
    // [SerializeField] SkinnedMeshRenderer playerSkinnedMeshRenderer;
    public playerController playerController;

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
    [SerializeField] private GameObject decisionUI;
    [SerializeField] private GameObject menuActive;
    public GameObject MenuInventory => menuInventory;
    public GameObject MenuActive => menuActive;
    private bool isPauseActive;
    public bool IsPauseActive => isPauseActive;

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

    [Header("Selected Game Objects UI")]
    [SerializeField] GameObject pauseQuitGameButton;
    [SerializeField] GameObject loseQuitGameButton;
    [SerializeField] GameObject winQuitGameButton;

    private int suspectListIndex;
    private int weaponListIndex;
    private int roomListIndex;
    private int motiveListIndex;

    public bool InventoryActive { get; private set; }
    public bool CraftTableActive { get; private set; }
    public bool DecisionActive { get; private set; }

    [Header("Day System")]
    [SerializeField] int _day;

    [Header("Timer System")]
    [SerializeField] float _time;
    public bool timerOn;
    public bool isTimeToSleep;
    public bool wentToSleep;
    public int Day => _day;

    float timeScaleOG;

    public int characterIndex;

    private Coroutine _coroutine = null;

    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else return;

        //DontDestroyOnLoad(this);


        menuActive = null;



        UpdateObjectiveText("Solve The Case");
        daySystemText.text = "Day " + _day;
        timerText.text = _time.ToString();

        timerOn = true;
        isTimeToSleep = false;
        wentToSleep = false;

        timeScaleOG = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        //playerSkinnedMeshRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        playerController = player.GetComponent<playerController>();

        characterIndex = PlayerPrefs.GetInt("SelectedCharacter");
        playerController.UpdatePlayerCharacter(characterIndex);
    }
    private void UpdateToggleUI(Toggle toggle, BaseClueData data)
    {
        TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = data.Name;
        toggle.image.sprite = data.Icon;
        
    }

    public void ClueFound(Toggle toggle)
    {
        toggle.isOn = true;
    }

    public void OnEnable()
    {
        EventSheet.SendAllClues += HandleClues;
        EventSheet.SendKillerClue += HandleKillerClue;
        EventSheet.SendClueToTracker += HandleNewClue;
     
    }
    public void OnDisable()
    {
        EventSheet.SendAllClues -= HandleClues;
        EventSheet.SendKillerClue -= HandleKillerClue;
        EventSheet.SendClueToTracker -= HandleNewClue;
    }

    public void HandleNewClue(Clue clue)
    {
        switch (clue.Data)
        {
            case KillerClueData suspect:
                
                ClueFound(killerToggles.ElementAt(killerChoices.IndexOf(suspect)));
                Destroy(clue.gameObject);
                break;
            case MotiveClueData motive:

                ClueFound(motiveToggles.ElementAt(motiveChoices.IndexOf(motive)));
                Destroy(clue.gameObject);
                break;
            case WeaponClueData weapon:

                ClueFound(weaponToggles.ElementAt(weaponChoices.IndexOf(weapon)));
                Destroy(clue.gameObject);
                break;
            case RoomClueData room:

                ClueFound(roomToggles.ElementAt(roomChoices.IndexOf(room)));
                Destroy(clue.gameObject);
                break;
        }
    }
    public void HandleClues(List<BaseClueData> clues)
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
    public void HandleKillerClue(KillerClueData clue)
    {
        
        killerChoices.Add(clue);
        UpdateToggleUI(killerToggles.ElementAt(killerChoices.IndexOf(clue)), clue);
    }

    //EventSheet.SendClueToTracker
    public void DisplayCharacterUI()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = characterUI;
            menuActive.SetActive(true);
        }
    }

    public void DeactivateCharacterUI()
    {
        characterUI.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        UpdateTimer(_time);  
        UpdateDayText(_day);
    }

    public void PauseGame()
    {
        isPauseActive = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        EventSystem.current.SetSelectedGameObject(pauseQuitGameButton);
        audioManager.instance.PauseSounds();
    }
    public void ActivatePauseMenu()
    {
        characterUI.SetActive(false);
        menuActive = pauseUI;
        menuActive.SetActive(true);
        audioManager.instance.PlaySFX(audioManager.instance.UIOpen, audioManager.instance.UIVol);
        
    }
    public void UnpauseGame()
    {
        isPauseActive = false;
        Time.timeScale = timeScaleOG;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        audioManager.instance.PauseSounds();
    }
    public void DeactivatePauseMenu()
    {
        menuActive.SetActive(false);
        menuActive = null;
        characterUI.SetActive(true);
        InputManager.instance.EnableCharacterInputs();
        audioManager.instance.PlaySFX(audioManager.instance.UIClose, audioManager.instance.UIVol);
    }
    public void ActivateInventoryUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuActive = menuInventory;
        menuActive.SetActive(true);
        InventoryActive = true;
        GetComponent<invManager>().IsUIActive = true;
        InputManager.instance.DisableCharacterInputs();
    }
    public void DeactivateInventoryUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<invManager>().IsUIActive = false;
        menuActive.SetActive(false);
        menuActive = null;       
        InventoryActive = false;
        InputManager.instance.EnableCharacterInputs();
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
        if (!TutorialUIManager.Instance.DisplayCraft)
        {
            TutorialUIManager.Instance.DisplayCraftTutorial();
        }
        InputManager.instance.DisableCharacterInputs();
    }
    public void DeactivateCraftTableUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        CraftTableActive = false;
        InputManager.instance.EnableCharacterInputs();
    }

    public void ActivateSleepMenu()
    {
        menuActive = sleepUI;
        if (!TutorialUIManager.Instance.DisplaySleeping)
        {
            TutorialUIManager.Instance.DisplaySleepingTutorial();
        }
        sleepUI.SetActive(true);
        //characterUI.SetActive(false);
    }

    public void DeactivateSleepMenu()
    {
        menuActive = characterUI;
        sleepUI.SetActive(false);
        //characterUI.SetActive(true);
    }
    public void ActivateDecisionUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuActive = decisionUI;
        if (!TutorialUIManager.Instance.DisplayVote)
        {
            TutorialUIManager.Instance.DisplayVotingTutorial();
        }
        menuActive.SetActive(true);
        DecisionActive = true;
    }
    public void DeactivateDecisionUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        DecisionActive = false;
    }

    public void WinGame()
    {
        PauseGame();

        menuActive = winUI;
        menuActive.SetActive(true);
        EventSystem.current.SetSelectedGameObject(winQuitGameButton);
        audioManager.instance.PlaySFX(audioManager.instance.UIWin, audioManager.instance.UIVol);
    }

    public void LoseGame()
    {
        PauseGame();
        menuActive = loseUI;
        menuActive.SetActive(true);
        EventSystem.current.SetSelectedGameObject(loseQuitGameButton);
        audioManager.instance.PlaySFX(audioManager.instance.UILose, audioManager.instance.UIVol);
    }

    public void OptionsMenu()
    {
        optionsUI.SetActive(true);
        menuActive = optionsUI;
        pauseUI.SetActive(false);
        ButtonFunctions.instance.LoadOptions();

        audioManager.instance.PlaySFX(audioManager.instance.UIOpen, audioManager.instance.UIVol);
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
            timerText.text = "";
            UpdateObjectiveText("Go To Sleep");
            isTimeToSleep = true;
            EventSheet.GateConditionStatus?.Invoke(false);
        }
        
    }

    public void TimeToGoToSleep()
    {


        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(Sleeping());            

        }
    }

    IEnumerator Sleeping()
    {

        //InputManager.instance.DisableCharacterInputs();
        ActivateSleepMenu();
        yield return new WaitForSeconds(1f);
        DeactivateSleepMenu();

        audioManager.instance.PlaySFX(audioManager.instance.sleeping, audioManager.instance.sleepVol);
        //InputManager.instance.EnableCharacterInputs();

        _day++;
        EventSheet.TodaysDayIndexIsThis.Invoke(_day);
        EventSheet.GateConditionStatus?.Invoke(true);
        isTimeToSleep = false;
        _coroutine = null;
        timerOn = true;
        UpdateTimer(15);
        UpdateObjectiveText("Solve The Case");
    }




}
