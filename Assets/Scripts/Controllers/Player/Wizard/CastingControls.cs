using UnityEngine;

public class CastingControls : MonoBehaviour
{
    [SerializeField]
    private Wand wand;

    private PlayerMovementScriptWizard controls;

    private bool fire = false;
    private bool cancel = false;

    private void Start()
    {
        controls = GameObject.FindObjectOfType<PlayerMovementScriptWizard>() as PlayerMovementScriptWizard;
    }

    private void FixedUpdate()
    {
        // After cancelling don't fire again until Mouse1 has been released
        if (cancel && controls.mousedown_1)
        {
            return;
        }
                
        fire = controls.mousedown_1;
        cancel = controls.mouse_2;

        if ((fire && !wand.casting))
        {
            wand.Fire(true);
        }
        else if ((!fire && wand.casting))
        {
            wand.Fire(false);
        }

        if (cancel)
        {
            wand.Cancel();
        }
    }
}
