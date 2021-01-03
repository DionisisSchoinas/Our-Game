using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeDisplay : MonoBehaviour
{
    private Image buttonImageCooldown;

    private void Awake()
    {
        buttonImageCooldown = gameObject.GetComponentsInChildren<Image>()[1];
        buttonImageCooldown.fillAmount = 0;
    }

    private void Start()
    {
        UIEventSystem.current.onDodgeFinish += Cooldown;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onDodgeFinish -= Cooldown;
    }

    private void Cooldown(float cooldown)
    {
        StartCoroutine(StartCooldown(cooldown));
    }

    IEnumerator StartCooldown(float cooldown)
    {
        float i = 0f;
        float delayForEachStep = cooldown / 50f;
        while (i < 1)
        {
            i += 0.02f;
            buttonImageCooldown.fillAmount += 0.02f;
            yield return new WaitForSeconds(delayForEachStep);
        }
        buttonImageCooldown.fillAmount = 0;
    }
}
