using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonFunctions : MonoBehaviour
{
    public static ButtonFunctions instance;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public Slider mouseSensitivity;
    public Slider controllerSensitivity;

    private float mouseSensitivityValue;
    private float controllerSensitivityValue;

    InputDevice activeDevice;

    [SerializeField] AudioMixer audioMixer;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        if (masterVolume == null) return;
        LoadOptions();
    }
    private void Update()
    {
        activeDevice = InputSystem.GetDevice<InputDevice>();

        UpdateSensitivity(activeDevice);
    }
    private void UpdateSensitivity(InputDevice activeDevice)
    {
        if (activeDevice is Keyboard || activeDevice is Mouse)
        {
            mouseSensitivity.interactable = true;
            controllerSensitivity.interactable = false;
        }
        else if (activeDevice is Gamepad)
        {
            mouseSensitivity.interactable = false;
            controllerSensitivity.interactable = true;
        }
    }

    public void ResumeGame()
    {
        GameManager.instance.UnpauseGame();
        GameManager.instance.DeactivatePauseMenu();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }

    public void OptionsButton()
    {
        GameManager.instance.OptionsMenu();
        EventSystem.current.SetSelectedGameObject(masterVolume.gameObject);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    public void SetMouseSensitivity(float value)
    {
        mouseSensitivityValue = value;
    }
    public void SetControllerSensitivity(float value)
    {
        controllerSensitivityValue = value;
    }

    public void CharacterSelectScreen()
    {
        MainMenuManager.instance.DisplayCharSelect(true);
    }

    public void BackToMainMenu()
    {
        MainMenuManager.instance.DisplayCharSelect(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenuOptions()
    {
        MainMenuManager.instance.OptionsMainMenu();
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivityValue);
        PlayerPrefs.SetFloat("ControllerSensitivity", controllerSensitivityValue);
        MainMenuOptions();

    }

    public void SaveGameOptions()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivityValue);
        PlayerPrefs.SetFloat("ControllerSensitivity", controllerSensitivityValue);
        GameManager.instance.ActivatePauseMenu();
    }

    public void LoadOptions()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
        mouseSensitivity.value = PlayerPrefs.GetFloat("MouseSensitivity");
        controllerSensitivity.value = PlayerPrefs.GetFloat("ControllerSensitivity");
    }

    public void StartCharacterSelect()
    {
        // CharacterSelectionManager.instance.DisplayCharacterSelect();
        SceneManager.LoadScene(1);
        
    }

    public void SaveSelectedCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", CharacterSelectionManager.instance.characterIndex);
        //GameManager.instance.DisplayCharacterUI();
        //GameManager.instance.playerController.UpdatePlayerCharacter(CharacterSelectionManager.instance.characterIndex);
    }

    public void UpdateCharacter(int index)
    {
        if (index < 0 
            || index > CharacterSelectionManager.instance.charactermodels.Count 
            || index == CharacterSelectionManager.instance.characterIndex) 
            return;

        Debug.Log("Character" + index + "selected");
        CharacterSelectionManager.instance.charactermodels[CharacterSelectionManager.instance.characterIndex].enabled = false;
        CharacterSelectionManager.instance.toggleList[CharacterSelectionManager.instance.characterIndex].isOn = false;

        CharacterSelectionManager.instance.characterIndex = index;
        CharacterSelectionManager.instance.charactermodels[CharacterSelectionManager.instance.characterIndex].enabled = true;
        CharacterSelectionManager.instance.toggleList[CharacterSelectionManager.instance.characterIndex].isOn = true;
    }

    public void DisplayCredits()
    {
        MainMenuManager.instance.DisplayCredits();
    }
    
    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.UnpauseGame();
    }
}
