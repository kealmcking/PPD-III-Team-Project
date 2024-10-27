using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerableDirectorManager : MonoBehaviour
{
    public static PlayerableDirectorManager instance;
    [SerializeField] GameObject inputText;
    [SerializeField] TextMeshProUGUI text;
    
    public PlayableDirector playableDirector;
    InputDevice activeDevice;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        activeDevice = InputSystem.GetDevice<InputDevice>();

        playableDirector = GetComponent<PlayableDirector>();

        if (activeDevice is Keyboard || activeDevice is Mouse)
        {
            inputText.SetActive(true);
            text = inputText.GetComponent<TextMeshProUGUI>();
            text.text = "Press 'X' to skip";
        }
        else if (activeDevice is Gamepad)
        {
            inputText.SetActive(true);
            text = inputText.GetComponent<TextMeshProUGUI>();
            text.text = "Press 'Select' to skip";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkipCutScene()
    {
        Debug.Log("Skip is activated");
        if (activeDevice is Keyboard || activeDevice is Mouse)
        {
            Debug.Log("X is pressed");
            inputText.SetActive(false);
            playableDirector.time = 30;
        }
        else if (activeDevice is Gamepad)
        {
            Debug.Log("Select is pressed");
            inputText.SetActive(false);
            playableDirector.time = 30;
        }
        
            
        
    }
}
