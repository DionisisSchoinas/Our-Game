using System.Collections.Generic;
using UnityEngine;
public class AnimationScriptControllerWarrior : MonoBehaviour
{
    public Animator animator;
    public CharacterController player;
    public Transform indicatorWheel;
    public float velocityZ, velocityX;


    private PlayerMovementScriptWarrior controls;

    List<string> combos = new List<string>(new string[] { "Combo1", "Combo2", "Combo3" });
    List<string> combosTriggers = new List<string>(new string[] { "Combo1 T", "Combo2 T", "Combo3 T" });
    public int combonum=0;
  
    // Start is called before the first frame update
    void Start()
    {

        controls = GameObject.FindObjectOfType<PlayerMovementScriptWarrior>() as PlayerMovementScriptWarrior;
        animator.SetLayerWeight(1, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //===== Movement Animations ======
        Vector3 direction = controls.direction;

        if (controls.isGrounded)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        direction = Quaternion.Euler(0, -indicatorWheel.eulerAngles.y, 0) * direction;
        float velocityRun = (controls.runspeed) / (controls.maxRunSpeed - controls.speed);
        velocityZ = (direction.z + velocityRun * direction.z);
        velocityX = (direction.x + velocityRun * direction.x);

        if (!controls.canMove)
        {
            animator.SetTrigger("HardLanding");
        }

        //===== Spell Casting Animations ======
        if (!controls.casting && !controls.rolling)
        {
            animator.SetBool("Rolling", false);
            if (direction != new Vector3(0f, 0f, 0f))
            {
                animator.SetFloat("Velocity X", 0f);
                animator.SetFloat("Velocity Z", 1 + velocityRun);
            }
            else
            {
                animator.SetFloat("Velocity Z", 0f);
                animator.SetFloat("Velocity X", 0f);
            }
        }
        else if (controls.casting)
        {
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }
        else if (controls.rolling)
        {
            animator.SetBool("Rolling", true);

            
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }
    }

    public void PlaySkillSelectAnimation()
    {
        animator.SetLayerWeight(1, 1);
        animator.SetBool("Cast Spell", true);
    }

    public void StopSkillSelectAnimation()
    {
        animator.SetBool("Cast Spell", false);
        animator.SetLayerWeight(1, 0);
    }

    public void Attack(int limit)
    {
        if (combonum < 3 && combonum < limit)
        {
            /*
            if (combonum != 0)
                animator.SetBool(combos[combonum-1], false);
            
            animator.SetBool(combos[combonum], true);
            */
            animator.SetTrigger(combosTriggers[combonum]);
            combonum++;
        }
    }

    public void ResetAttack()
    {
        /*
        for (int cnum = 0; cnum < 3; cnum++)
        {
            animator.SetBool(combos[cnum], false);
        }
        */
        combonum = 0;
    }

}

