using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterDB characterDB;
    [SerializeField] private Transform characterSpawnPoint;
    [SerializeField] GameObject playerObj;
    private int selectedOption = 0;

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
    private void UpdateCharacter(int selectedOption)
    {
        if (playerObj != null)
        {
            Destroy(playerObj);
        }
        Character character = characterDB.GetCharacter(selectedOption);
        // Instantiate the character model at the spawn point
        playerObj = Instantiate(character.characterModel, characterSpawnPoint.position, characterSpawnPoint.rotation);
        // Pass the instantiated model to the playerController
        playerController pc = playerObj.GetComponent<playerController>();
        pc.SetCharacterModel(playerObj);
    }
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

}