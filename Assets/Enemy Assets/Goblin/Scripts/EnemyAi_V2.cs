using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi_V2 : MonoBehaviour
{

    public float lookRadius = 20f;
  
    public float attackRadius = 5f;

    [HideInInspector]
    public Transform target;
   
  
    public bool hasTarget;

    //Scripts And GameObjects
    [SerializeField]
    public GameObject destinationIndicator;
    private EnemyMeleeController meleeController;
    private ProjectileManager rangedController;
    Vector3 walkpoint;
    NavMeshAgent agent;
   

    private Vector3 spawnpoint;
   
    //State Machine Variables
    public bool targetReached;
  
    void Start()
    {
        GameObject[] transforms = GameObject.FindGameObjectsWithTag("Player");
        if (transforms.Length != 0)
            target = transforms[0].transform;

        agent = GetComponent<NavMeshAgent>();
        
        //initiaslize spawnpoint
        spawnpoint = transform.position;
       
        //initialize the melee Controller 
        meleeController = GetComponent<EnemyMeleeController>();

        //initialize the ranged Controller 
        rangedController = GetComponent<ProjectileManager>();
    }


    public void Patrol()
    {
        float distance = Vector3.Distance(walkpoint, transform.position);

        if (distance < agent.stoppingDistance)
        {
           Stop();
        }
    }


    public void Stop()
    {
        hasTarget = false;
        agent.ResetPath();
    }
  
    public void SearchForTarget()
    {
        float randomZ = Random.Range(-lookRadius, lookRadius);
        float randomX = Random.Range(-lookRadius, lookRadius);

        walkpoint = new Vector3(spawnpoint.x + randomX, 2, spawnpoint.z + randomZ);

        findPoint(walkpoint);
    }

    public void SearchForTargetNearPlayer()
    {
        //square around player
        float randomZ = Random.Range(-2, 2);
        float randomX = Random.Range(-2, 2);

        //offset so enemy is not too close to player
        if (randomZ > 0) randomZ += lookRadius/2;
        else randomZ -= lookRadius / 2;

        if (randomX > 0) randomX += lookRadius / 2;
        else randomX -= lookRadius / 2;

        walkpoint = new Vector3(target.transform.position.x + randomX, 2, target.transform.position.z + randomZ);

        findPoint(walkpoint);
    }

    private NavMeshHit findPoint(Vector3 walkpoint)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(walkpoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //Destroy(Instantiate(destinationIndicator, hit.position + new Vector3(0, 0.6f, 0), destinationIndicator.transform.rotation), 2f);
            agent.SetDestination(hit.position);
            hasTarget = true;
        }
        return hit;
    }

    public void Chase()
    {
        agent.SetDestination(target.position);
    }

    public void Relocate()
    {
        if (!hasTarget)
        {
            SearchForTargetNearPlayer();
        }
        else
        {
            float distance = Vector3.Distance(walkpoint, transform.position);

            if (distance < agent.stoppingDistance)
            {
                targetReached = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        agent = GetComponent<NavMeshAgent>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
    }

    public void rotateTowards(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.eulerAngles = rotation.eulerAngles;
    }

    public void Attack()
    {
        meleeController.canAttack = true;
    }

    public void AttackRanged()
    {
        rangedController.canAttack = true;
    }
}