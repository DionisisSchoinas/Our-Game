using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityResource : MonoBehaviour
{
    public float maxValue = 100f;
    public ResourceBar resourceBar;
    public float regenPerSecond;

    private Coroutine regenCoroutine;
    private float lastReduction;

    private Color barColor;
    private bool regening;
    private bool finsihedRegen;

    private float _currentValue;
    public float currentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            // Check if the resource was reduced
            if (_currentValue > value)
            {
                lastReduction = Time.time;
                regening = false;

                if (regenCoroutine != null)
                    StopCoroutine(regenCoroutine);
            }

            _currentValue = Mathf.Min(value, maxValue);

            if (resourceBar != null)
                resourceBar.SetValue(value);
        }
    }

    protected void Awake()
    {
        regenCoroutine = null;
        regening = false;
        finsihedRegen = false;
    }

    protected void Start()
    {
        if (resourceBar != null)
        {
            resourceBar.SetColor(barColor);
            resourceBar.SetMaxValue(maxValue);
        }
        currentValue = maxValue;
    }

    public void SetValues(float maxValue, float regenPerSecond, ResourceBar resourceBar, Color barColor)
    {
        this.maxValue = maxValue;
        this.resourceBar = resourceBar;
        this.regenPerSecond = regenPerSecond;
        this.barColor = barColor;
    }

    protected void FixedUpdate()
    {
        if (currentValue < maxValue && !regening && Time.time - lastReduction >= 2f)
        {
            regening = true;
            regenCoroutine = StartCoroutine(Regen());
        }
        else if (finsihedRegen && regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
            regening = false;
        }
    }

    private IEnumerator Regen()
    {
        finsihedRegen = false;
        while (currentValue < maxValue)
        {
            currentValue = currentValue + regenPerSecond / 10f;
            yield return new WaitForSeconds(0.1f);
        }
        finsihedRegen = true;
        yield return null;
    }
}
