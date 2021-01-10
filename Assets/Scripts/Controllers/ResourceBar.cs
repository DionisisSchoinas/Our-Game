using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    public Color resourceBarColor;
    public Slider resourceBar;

    private Image resourceDisplay;
    private float maxValue;

    private void Awake()
    {
        resourceDisplay = GetComponent<Image>();
        if (resourceDisplay != null)
        {
            resourceDisplay.color = resourceBarColor;
            resourceDisplay.fillAmount = 1f;
        }
    }

    public void SetMaxValue(float max)
    {
        if (resourceBar != null)
            resourceBar.maxValue = max;

        if (resourceDisplay != null)
            resourceDisplay.fillAmount = 1f;

        maxValue = max;
    }

    public void SetValue(float value)
    {
        if (resourceBar != null)
            resourceBar.value = value;

        if (resourceDisplay != null)
            resourceDisplay.fillAmount = value / maxValue;
    }
}
