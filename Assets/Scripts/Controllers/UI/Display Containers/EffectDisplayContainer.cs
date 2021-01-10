using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectDisplayContainer : MonoBehaviour
{
    private Image cooldownDisplay;
    private Text textDisplay;
    private Coroutine coroutine;
    private float cooldownPercentage;
    private bool resistance;
    private bool damage;

    private void Awake()
    {
        textDisplay = gameObject.GetComponentInChildren<Text>();

        cooldownDisplay = gameObject.GetComponentsInChildren<Image>()[1];
        cooldownDisplay.fillAmount = 1f;
        cooldownPercentage = 1f;

        coroutine = null;
        resistance = false;
        damage = false;

        UIEventSystem.current.onRemoveResistance += RemoveResistance;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onRemoveResistance -= RemoveResistance;
    }

    public void SetResistanceText(string text)
    {
        textDisplay.text = text;
        resistance = true;
        damage = false;
    }

    public void SetDamageTypeText(string text)
    {
        textDisplay.text = text;
        damage = true;
        resistance = false;
    }

    public void StartCountdown(float duration)
    {
        coroutine = StartCoroutine(StartTimer(duration));
    }

    private IEnumerator StartTimer(float duration)
    {
        cooldownPercentage = 1f;

        float delayForEachStep = duration / 100f;
        while (cooldownPercentage > 0)
        {
            cooldownPercentage -= 0.01f;
            cooldownDisplay.fillAmount = cooldownPercentage;
            yield return new WaitForSeconds(delayForEachStep);
        }

        cooldownPercentage = 1f;
    }

    private void RemoveResistance()
    {
        if (resistance)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            Destroy(gameObject);
        }
    }
}
