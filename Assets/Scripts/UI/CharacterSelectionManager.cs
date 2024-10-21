using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public List<Texture> characterIcon = new List<Texture>();

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
        
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].transform.Find("Background/Checkmark").GetComponent<Image>().gameObject.SetActive(false);
            toggleList[i].transform.Find("Background/Icon").GetComponent<RawImage>().texture = characterIcon[i];

            if (!toggleList[i].interactable)
            {
                toggleList[i].transform.Find("Background/Icon").GetComponent<RawImage>().color = Color.black;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        //public void DisplayCharacterSelect()
        //{





        //}





    }
