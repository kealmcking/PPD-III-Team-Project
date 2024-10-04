using Input;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        masterVolume.AddComponent<Selectable>();
        musicVolume.AddComponent<Selectable>();
        sfxVolume.AddComponent<Selectable>();
        

        
        
    }

    public void ResumeGame()
    {
        InputManager.instance.SetIsPause(false);
        GameManager.instance.UnpauseGame();
        InputManager.instance.EnableCharacterInputs();
        
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(0);
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
}
