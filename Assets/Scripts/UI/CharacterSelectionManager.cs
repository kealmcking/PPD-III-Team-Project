using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{

    public static CharacterSelectionManager instance;
    [SerializeField] public GameObject menuActive;
    [SerializeField] private GameObject characterSelectUI;

    [Header("Lists")]
    public List<SkinnedMeshRenderer> charactermodels = new List<SkinnedMeshRenderer>();
    public List<Toggle> toggleList = new List<Toggle>();

    public int characterIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
      //  DisplayCharacterSelect();
        charactermodels[characterIndex].enabled = true;
        instance.toggleList[characterIndex].isOn = true;
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter");
        foreach (Toggle toggle in toggleList)
        {
            toggle.isOn = false;
        }
        EventSystem.current.SetSelectedGameObject(toggleList[characterIndex].gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        //public void DisplayCharacterSelect()
        //{





        //}





    }
