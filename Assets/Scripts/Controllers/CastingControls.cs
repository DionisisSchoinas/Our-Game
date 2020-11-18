using UnityEngine;

public class CastingControls : MonoBehaviour
{
    [SerializeField]
    private Wand wand;
    [SerializeField]
    private OverlayController overlayController;

    private PlayerMovementScript controls;

    private bool fire1 = false;
    private bool fire2 = false;

    private void Start()
    {
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
    }

    private void FixedUpdate()
    {
        if (controls.menu && !overlayController.isEnabled)
        {
            overlayController.Enable(true);
            fire1 = false;
            fire2 = false;
        }
        else if (!controls.menu && overlayController.isEnabled)
        {
            overlayController.Enable(false);
        }

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
