using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField]
    private Transform firepoint;
    [SerializeField]
    private ProjectileScript projectile;
    public bool canAttack = false;
    public float attackDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
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
        projectile.FireSimple(firepoint);
        yield return new WaitForSeconds(0.1f);




    }
}
