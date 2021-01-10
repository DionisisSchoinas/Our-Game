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

    [SerializeField]
    private float _currentValue;
    public float currentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            // Check if the resorce was reduced
            if (_currentValue > value) 
                lastReduction = Time.time;

            _currentValue = value;
            resourceBar.SetValue(value);
        }
    }

    protected void Awake()
    {
        regenCoroutine = null;
    }

    public void SetValues(float maxValue, float regenPerSecond, ResourceBar resourceBar)
    {
        this.maxValue = maxValue;
        this.resourceBar = resourceBar;
        this.regenPerSecond = regenPerSecond;

        this.resourceBar.SetMaxValue(maxValue);
        currentValue = maxValue;

        Debug.Log("function");
    }

    protected void FixedUpdate()
    {
        if (currentValue < maxValue && regenCoroutine == null && Time.time - lastReduction >= 2f)
        {
            regenCoroutine = StartCoroutine(Regen());
        }
        else if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    private IEnumerator Regen()
    {
        while (currentValue < maxValue)
        {
            currentValue = currentValue + regenPerSecond / 10f;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
