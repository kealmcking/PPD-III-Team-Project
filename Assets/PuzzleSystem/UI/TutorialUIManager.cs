using Input;
using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager Instance;
    [SerializeField] GameObject craftingTable;
    [SerializeField] GameObject blockedArea;
    [SerializeField] GameObject voting;
    [SerializeField] GameObject dialogue;
    [SerializeField] GameObject sleeping;
    [SerializeField] GameObject gameplay;
    
    public bool DisplayCraft { get; set; } = false;
    public bool DisplayBlocked { get; set; } = false;
    public bool DisplayVote { get; set; } = false;
    public bool DisplayDialogue { get; set; } = false;
    public bool DisplaySleeping { get; set; } = false;
    public bool DisplayGameplay { get; set; } = false;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(this);
        }
    }
    public void Start()
    {    
          DisplayGameplayTutorial();     
    }
    public void DisplayCraftTutorial()
    {
        if (!DisplayCraft)
        {           
            craftingTable.SetActive(true);
            DisplayCraft = true;
        }
    }
    public void DisplayBlockedArea()
    {
        if (!DisplayBlocked)
        {
            InputManager.instance.DisableCharacterInputs();
            blockedArea.SetActive(true);
            DisplayBlocked = true;
        }
    }
    public void DisplayDialogueTutorial()
    {
        if (!DisplayDialogue)
        {            
            dialogue.SetActive(true);
            DisplayDialogue = true;
        }
        
    }
    public void DisplaySleepingTutorial()
    {
        if (!DisplaySleeping)
        {
            GameManager.instance.PauseGame();
            sleeping.SetActive(true);
            DisplaySleeping = true;
        }

        
    }
    public void DisplayVotingTutorial()
    {
        if (!DisplayVote)
        {
           
            voting.SetActive(true);
            DisplayVote = true;
        }
       
    }
    public void DisplayGameplayTutorial()
    {
        if (!DisplayGameplay)
        {
            InputManager.instance.DisableCharacterInputs();
            gameplay.SetActive(true);
            DisplayGameplay = true;
                    
        }
        
    }
    public void CloseBlocked()
    {
        blockedArea.SetActive(false);
        InputManager.instance.EnableCharacterInputs();
    }
    public void CloseGameplay()
    {
        gameplay.SetActive(false);
        InputManager.instance.EnableCharacterInputs();
    }
    public void CloseVoting()
    {
        voting.SetActive(false);
    }
    public void CloseSleeping()
    {
        sleeping.SetActive(false);
        GameManager.instance.UnpauseGame();
    }
    public void CloseCraft()
    {
        craftingTable.SetActive(false);
    }
    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }
}
