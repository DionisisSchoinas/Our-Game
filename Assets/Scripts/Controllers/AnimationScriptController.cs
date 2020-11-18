using System.Collections;
using UnityEngine;


public class AnimationScriptController : MonoBehaviour
{
    public Animator animator;
    public CharacterController player;
    public Transform indicatorWheel;
    public float velocityZ,velocityX;
    public GameObject handForBasicSpells;
    public GameObject leftHandForChannelingSpells;
    public GameObject rightHandForChannelingSpells;
    public bool allowStopCasting;

    private PlayerMovementScript controls;
    private ParticleSystem tmpSource;

    // Start is called before the first frame update
    void Start()
    {
        allowStopCasting = true;
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
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
            animator.ResetTrigger("HardLanding");
            animator.SetBool("IsGrounded", false);
        }
       
        direction = Quaternion.Euler(0, -indicatorWheel.eulerAngles.y, 0) * direction;
        float velocityRun= ( controls.runspeed) / ( controls.maxRunSpeed- controls.speed);
        velocityZ = (direction.z+  velocityRun * direction.z);
        velocityX = (direction.x + velocityRun * direction.x);

        if (!controls.canMove)
        {
            animator.SetTrigger("HardLanding");
        }

        //===== Spell Casting Animations ======
        if (!controls.casting)
        {
            //fireboltHand.SetActive(false);
            animator.SetLayerWeight(1, 0);
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
        else
        {
            //fireboltHand.SetActive(true);
            animator.SetLayerWeight(1, 1);
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }
        
    }

    public void ChargeBasic(ParticleSystem source)
    {
        animator.SetBool("ReleaseBasic", false);
        animator.SetBool("ChargeBasic", true);
        tmpSource = Instantiate(source, handForBasicSpells.transform);
        //tmpSource.transform.position += Vector3.left * 0.3f;
    }
    public void ReleaseBasic()
    {
        animator.SetBool("ChargeBasic", false);
        animator.SetBool("ReleaseBasic", true);
    }

    public void HideSource()
    {
        Destroy(tmpSource.gameObject);
    }

    public void CastChannel(bool channeling, ParticleSystem source, float chargeUp, float reset)
    {
        animator.SetBool("Chanelling", channeling);
        StartCoroutine(CastChannelingAnimation(channeling, source, chargeUp, reset));
    }

    IEnumerator CastBasicAnimation(ParticleSystem source, float chargeUp, float reset)
    {
        allowStopCasting = false;
        ParticleSystem tmp = Instantiate(source, handForBasicSpells.transform);
        tmp.transform.position += Vector3.left * 0.1f;
        yield return new WaitForSeconds(chargeUp);
        Destroy(tmp.gameObject);
        yield return new WaitForSeconds(reset);
        allowStopCasting = true;
    }

    IEnumerator CastChannelingAnimation(bool holding, ParticleSystem source, float chargeUp, float reset)
    {
        //if starting to channel spell
        if (holding)
        {
            allowStopCasting = false;
            //create source particle for each hand
            ParticleSystem[] particles = new ParticleSystem[2];
            particles[0] = Instantiate(source, leftHandForChannelingSpells.transform);
            particles[0].transform.position += Vector3.right * 0.1f;
            particles[1] = Instantiate(source, rightHandForChannelingSpells.transform);
            particles[1].transform.position += Vector3.left * 0.1f;
            yield return new WaitForSeconds(chargeUp);
            //destroy all particles
            foreach (ParticleSystem pr in particles)
            {
                Destroy(pr.gameObject);
            }
        }
        //if finishing channel
        else
        {
            yield return new WaitForSeconds(reset);
            allowStopCasting = true;
        }
    }
}
