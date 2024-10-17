using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public static ButtonFunctions instance;
    //public AudioMixer audioMixer;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        
        LoadOptions();
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
    }

    public void SetMasterVolume(float volume)
    {
        //audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        //audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        //audioMixer.SetFloat("SFXVolume", volume);
    }

    public void CharacterSelectScreen()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
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
        MainMenuOptions();

    }

    public void LoadOptions()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
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
    
}
