using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    public CharacterDB characterDB;

    public TextMeshProUGUI nameText;
    //model reference
    public RawImage characterOption;
    public Texture lockedCharacter;
    private int selectedOption = 0;
    public GameObject characterPositionUI;

    // Start is called before the first frame update
    void Start()
    {
        //checks to make sure there isnt save data
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }

        UpdateCharacter(selectedOption);

    }
    //iterate through choices
    public void NextOption()
    {
        selectedOption++;

        if (selectedOption >= characterDB.characterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
        Save();
    }

    public void PrevOption()
    {
        selectedOption--;

        if (selectedOption < 0)
        {
            selectedOption = characterDB.characterCount - 1;
        }

        UpdateCharacter(selectedOption);
        Save();
    }
    //assign character values
    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        if (character.isUnlocked)
        {
            characterOption.transform.localScale = Vector3.one;
            characterOption.color = Color.white;
            characterOption.texture = character.characterDisplayUI;
            nameText.text = character.characterName;
        }
        else
        {
            characterOption.transform.localScale = Vector3.one * 0.25f;
            characterOption.color = Color.red;
            characterOption.texture = lockedCharacter;
            nameText.text = "???";
        }
    }
    //methods for storing data on variables used by the player
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    public void UnlockCharacter(int index)
    {
        Character character = characterDB.GetCharacter(index);
        character.isUnlocked = true;
        PlayerPrefs.SetInt("Character_" + index + "_unlocked", 1);
    }

    private void LoadUnlocks()
    {
        for (int i = 0; i < characterDB.characterCount; i++)
        {
            if (PlayerPrefs.HasKey("Character_" + i + "_unlocked"))
            {
                characterDB.GetCharacter(i).isUnlocked = PlayerPrefs.GetInt("Character_" + i + "_unlocked") == 1;
            }
            else
            {
                characterDB.GetCharacter(i).isUnlocked = false;
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        Character character = characterDB.GetCharacter(selectedOption);

        if (character.isUnlocked)
        {
            // Set the selected character in the playerController
            playerController pc = FindFirstObjectByType<playerController>();
            if (pc != null)
            {
                pc._character = character; // Pass the selected character to the controller
            }

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("This character is locked!");
        }
    }


}