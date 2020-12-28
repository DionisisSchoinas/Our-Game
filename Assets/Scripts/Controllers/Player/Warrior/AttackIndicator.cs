using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    //Links : https://www.youtube.com/watch?v=rQG9aUWarwE
   
    public float attackRadius;
    [Range (0,360)]
    public float attackAngle;

    public LayerMask damageablesMask;
  
    public List<Transform> visibleTargets = new List<Transform>();

    void FixedUpdate() 
    {
        FindTargets();
    }
    void FindTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, attackRadius, damageablesMask);
        for (int i = 0; i < targetsInRange.Length; i++)
        {
            if (targetsInRange[i].tag != gameObject.tag)
            { 
                Transform target = targetsInRange[i].transform;
                Vector3 dirToTarget = (target.position - transform.position);
                if (Vector3.Angle(transform.forward, dirToTarget) < attackAngle / 2)
                {
                    visibleTargets.Add(target);
                  
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees,bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
