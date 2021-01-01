using UnityEngine;

public class CastingControls : MonoBehaviour
{
    [SerializeField]
    private Wand wand;

    private PlayerMovementScriptWizard controls;

    private bool fire1 = false;
    private bool fire2 = false;

    private void Start()
    {
        controls = GameObject.FindObjectOfType<PlayerMovementScriptWizard>() as PlayerMovementScriptWizard;
    }

    private void FixedUpdate()
    {
        if (!controls.menu)
        {
            fire1 = controls.mousedown_1;
            fire2 = controls.mousedown_2;

        }

        if (fire1 && !Wand.castingBasic)
        {
            wand.Fire1(true);
        }
        else if (!fire1 && Wand.canRelease)
        {
            wand.Fire1(false);
        }

        if (fire2 && !Wand.channeling)
        {
            wand.Fire2(true);
        }
        else if (!fire2 && Wand.channeling)
        {
            wand.Fire2(false);
        }
    }
}
