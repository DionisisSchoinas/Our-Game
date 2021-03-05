using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    private Slider resourceBar;
    private Color resourceBarColor;

    private Image resourceDisplay;
    private float maxValue;

    private void Awake()
    {
        resourceBar = GetComponent<Slider>();
        resourceDisplay = GetComponent<Image>();
        if (resourceDisplay != null)
        {
            resourceDisplay.color = resourceBarColor;
            resourceDisplay.fillAmount = 1f;
        }
    }

    public void SetColor(Color barColor)
    {
        if (resourceDisplay != null)
        {
            resourceBarColor = barColor;
            resourceDisplay.color = barColor;
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
        {
            resourceBar.value = value;
            if (resourceBar.value <= 0f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        if (resourceDisplay != null)
        {
            resourceDisplay.fillAmount = value / maxValue;
        }
    }
}
