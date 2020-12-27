using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;

    public void SetMaxHealth(float health)
    {
        if (healthBar != null)
            healthBar.maxValue = health;
    }

    public void SetHealth(float health)
    {
        if (healthBar != null)
            healthBar.value = health;
    }
}
