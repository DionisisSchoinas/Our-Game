using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private void Start()
    {
        UIEventSystem.current.onStartCooldown += StartCooldown;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onStartCooldown -= StartCooldown;
    }

    public void StartCooldown(Skill skill, float delay)
    {
        StartCoroutine(Cooldown(skill, delay));
    }

    private IEnumerator Cooldown(Skill skill, float delay)
    {
        skill.onCooldown = true;
        skill.cooldownPercentage = 0f;

        float delayForEachStep = delay / 100f;
        while (skill.cooldownPercentage < 1)
        {
            skill.cooldownPercentage += 0.01f;
            yield return new WaitForSeconds(delayForEachStep);
        }

        skill.cooldownPercentage = 0f;
        skill.onCooldown = false;

        yield return null;
    }
}
