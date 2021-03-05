using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    private GameObject CurrentGameObject;
    private EnemyAi_V2 aiScript;
    private Transform target;
    public float maxPatrolTimer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CurrentGameObject = animator.gameObject;
        aiScript = CurrentGameObject.GetComponent<EnemyAi_V2>();
        target = aiScript.target.transform;
        maxPatrolTimer = 0;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiScript.Patrol();
        //give up going to target
        maxPatrolTimer += Time.deltaTime;
        if (maxPatrolTimer > 3f)
        {
            aiScript.Stop();
            animator.SetBool("Patrolling", false);
        }
        
        
        if (Vector3.Distance(target.transform.position, animator.transform.position) <= aiScript.lookRadius)
        {
            animator.SetBool("Chase", true);
        }
        else if (!aiScript.hasTarget)
        {
            
            animator.SetBool("Patrolling", false);
        }
    
    }

     
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
