using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoadScript : MonoBehaviour
{
    private void Awake()
    {
        SelectedCharacterScript selectedCharacter = FindObjectOfType<SelectedCharacterScript>();
        if (selectedCharacter != null)
        {
            Debug.Log(FindObjectOfType<SelectedCharacterScript>().character);

            GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");

            string character_name = "";

            switch (selectedCharacter.character)
            {
                case SelectedCharacterScript.Character.Fighter:
                    character_name = "Player (Warrior)";
                    break;
                default:
                    character_name = "Player (Wizard)";
                    break;
            }

            foreach (GameObject gm in characters)
            {
                if (gm.name.Equals(character_name))
                {
                    gm.SetActive(true);
                }
                else
                {
                    gm.SetActive(false);
                }
            }
        }
    }
}
