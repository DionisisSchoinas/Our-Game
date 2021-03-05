using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    AttackIndicator indicator;
    public bool canAttack = true;//check if the cooldown has passed
    public float attackDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        indicator = GetComponent<AttackIndicator>() as AttackIndicator;
    

    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            StartCoroutine(PerformAttack());
            canAttack = false;
        }
    }

    IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(attackDelay);

        foreach (Transform visibleTarget in indicator.visibleTargets)
        {
            Debug.Log(visibleTarget.name);

            HealthEventSystem.current.TakeDamage(visibleTarget.name, 15f, DamageTypesManager.Physical);
            CameraShake.current.ShakeCamera(0.05f, 0.2f);

        }

        yield return new WaitForSeconds(0.1f);
    }
}
