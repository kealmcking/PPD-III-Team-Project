using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;

    [SerializeField] private GameObject optionsMainMenu;
    [SerializeField] private GameObject contents;
    [SerializeField] private GameObject masterVolume;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] public GameObject menuActive;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject backButton;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) return;
        instance = this;
        DisplayMainMenu();
    }
    public void DisplayMainMenu()
    {
        if (menuActive == null)
        {

            menuActive = mainMenu;
            // menuActive.SetActive(true);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
        //else
        //{
        //    UnpauseGame();
        //}
    }
    public void DisplayCredits()
    {
        if(menuActive == mainMenu)
        {
            menuActive = creditsUI;
            menuActive.SetActive(true);
            EventSystem.current.SetSelectedGameObject(backButton);
        }
        else if(menuActive == creditsUI)
        {
            menuActive.SetActive(false);
            menuActive = mainMenu;
            menuActive.SetActive(true);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void OptionsMainMenu()
    {
        ButtonFunctions.instance.LoadOptions();
        if (menuActive == mainMenu)
        {
            contents.SetActive(false);
            menuActive = optionsMainMenu;
            menuActive.SetActive(true);
            EventSystem.current.SetSelectedGameObject(masterVolume);


        }
        else if (menuActive == optionsMainMenu)
        {
            menuActive.SetActive(false);
            menuActive = mainMenu;
            contents.SetActive(true);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }
}
