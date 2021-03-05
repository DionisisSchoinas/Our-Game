using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerEnemyV2 : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.GetBool("Chase") || stateMachine.GetBool("Relocate"))
        {
            animator.SetBool("Walking", true);
        }
        else if (stateMachine.GetBool("Engage"))
        {
            animator.SetBool("Walking", false);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
