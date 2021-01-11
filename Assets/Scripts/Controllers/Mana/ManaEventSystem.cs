using System;
using UnityEngine;

public class ManaEventSystem : MonoBehaviour
{

    public static ManaEventSystem current;

    private void Awake()
    {
        current = this;
    }

    // When mana is used
    public event Action<float> onManaUsed;
    public void UseMana(float mana)
    {
        if (onManaUsed != null)
        {
            onManaUsed(mana);
        }
    }

    // Sends out the current mana value
    public event Action<float> onManaUpdated;
    public void UpdateMana(float currentMana)
    {
        if (onManaUpdated != null)
        {
            onManaUpdated(currentMana);
        }
    }
}
