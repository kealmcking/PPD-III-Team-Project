using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{

    public static CharacterSelectionManager instance;

    [SerializeField] List<SkinnedMeshRenderer> charactermodels = new List<SkinnedMeshRenderer>();
    [SerializeField] List<Toggle> toggleList = new List<Toggle>();

    
    

    public int characterIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Toggle toggle in toggleList)
        {
            toggle.isOn = false;
        }



        characterIndex = PlayerPrefs.GetInt("SelectedCharacter");
        charactermodels[characterIndex].enabled = true;
        toggleList[characterIndex].isOn = true;
        EventSystem.current.SetSelectedGameObject(toggleList[characterIndex].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCharacter(int index)
    {
        if (index < 0 || index > charactermodels.Count || index == characterIndex) return;

        charactermodels[characterIndex].enabled = false;
         toggleList[characterIndex].isOn = false;

        characterIndex = index;
        charactermodels[characterIndex].enabled = true;
         toggleList[characterIndex].isOn = true;
    }
    public void SaveSelectedCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        //GameManager.instance.UpdatePlayerCharacter(charactermodels[characterIndex]);
       // SceneManager.LoadScene(2);
    }

    
}
