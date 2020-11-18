using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ConditionsHandler : MonoBehaviour
{
    private List<Condition> conditions;
    private List<Coroutine> conditionsCoroutines;
    private List<ParticleSystem> conditionsParticles;

    private void Start()
    {
        conditions = new List<Condition>();
        conditionsCoroutines = new List<Coroutine>();
        conditionsParticles = new List<ParticleSystem>();
    }

    public void AddCondition(Condition condition)
    {
        if (conditions.Contains(condition))
        {
            // Remove old instance
            int cur_con = conditions.IndexOf(condition);
            StopCoroutine(conditionsCoroutines[cur_con]); // Stop coroutine
            conditionsCoroutines.RemoveAt(cur_con); // Delete coroutine
            Destroy(conditionsParticles[cur_con].gameObject); // Destroy particles
            conditionsParticles.RemoveAt(cur_con); // Delete particles
            conditions.Remove(condition); // Delete condition

        }
        // Add new instance
        conditions.Add(condition);
        conditionsParticles.Add(Instantiate(condition.effect, gameObject.transform));
        conditionsParticles[conditionsParticles.Count - 1].transform.localScale = transform.localScale;
        conditionsCoroutines.Add(StartCoroutine(nameof(DamageOverTime), condition));
    }

    IEnumerator DamageOverTime(Condition condition)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        while (watch.Elapsed.TotalSeconds <= condition.duration)
        {
            HealthEventSystem.current.TakeDamageIgnoreShields(gameObject.name, condition.damage, condition.damageType);
            yield return new WaitForSeconds(1f / condition.damageTicks);
        }

        watch.Stop();

        int cur_con = conditions.IndexOf(condition);
        Destroy(conditionsParticles[cur_con].gameObject); // Destroy particles
        conditionsCoroutines.RemoveAt(cur_con); // Delete coroutine
        conditionsParticles.RemoveAt(cur_con); // Delete particles
        conditions.Remove(condition); // Delete condition
        yield return null;
    }
}
