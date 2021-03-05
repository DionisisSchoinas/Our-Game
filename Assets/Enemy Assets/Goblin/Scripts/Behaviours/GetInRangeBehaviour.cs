using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInRangeBehaviour : StateMachineBehaviour
{
    private GameObject CurrentGameObject;
    private EnemyAi_V2 aiScript;
    private Transform target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CurrentGameObject = animator.gameObject;
        aiScript = CurrentGameObject.GetComponent<EnemyAi_V2>();
        target = aiScript.target.transform;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiScript.Chase();
        if (Vector3.Distance(target.transform.position, animator.transform.position) <= aiScript.attackRadius-1 )
        {
            aiScript.Stop();
            animator.SetBool("GetInRange", false);


        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
