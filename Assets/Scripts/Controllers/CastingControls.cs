using UnityEngine;

public class CastingControls : MonoBehaviour
{
    [SerializeField]
    private Wand wand;

    private PlayerMovementScriptWizard controls;

    private bool fire = false;

    private void Start()
    {
        controls = GameObject.FindObjectOfType<PlayerMovementScriptWizard>() as PlayerMovementScriptWizard;
    }

    private void FixedUpdate()
    {
        if (!controls.menu)
        {
            fire = controls.mousedown_1;
        }

        if ((fire && !Wand.casting))
        {
            wand.Fire(true);
        }
        else if ((!fire && Wand.casting))
        {
            wand.Fire(false);
        }
    }
}
