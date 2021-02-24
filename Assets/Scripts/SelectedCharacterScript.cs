using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCharacterScript : MonoBehaviour
{
    [HideInInspector]
    public enum Character
    {
        Wizard,
        Fighter
    }

    [HideInInspector]
    public Character character;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
    }
}
