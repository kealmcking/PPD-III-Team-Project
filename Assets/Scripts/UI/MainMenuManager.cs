using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject menuActive;
    [SerializeField] private GameObject contents;
    [SerializeField] private GameObject masterVolume;


    [SerializeField] private GameObject playButton;
   

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) return;
        instance = this;
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void OptionsMenu()
    {
          
        if (menuActive == null)
        {
            contents.SetActive(false);
            menuActive = optionsMenu;
            menuActive.SetActive(true);
            EventSystem.current.SetSelectedGameObject(masterVolume);
           
             
        }
        else if (menuActive == optionsMenu)
        {
            menuActive.SetActive(false);
            menuActive = null;
           contents.SetActive(true);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }
}
