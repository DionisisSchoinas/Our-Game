using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform tipPoint;
    public Transform basePoint;
    public SwordEffect[] swordEffects;

    private int selectedEffect;
    private SwordEffect currentEffect;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        selectedEffect = 0;
        ChangeSwordEffect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            selectedEffect = 0;
            ChangeSwordEffect();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedEffect = 1;
            ChangeSwordEffect();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            selectedEffect = 2;
            ChangeSwordEffect();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            selectedEffect = 3;
            ChangeSwordEffect();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            selectedEffect = 4;
            ChangeSwordEffect();
        }
    }

    private void ChangeSwordEffect()
    {
        if (currentEffect != null) Destroy(currentEffect.gameObject);
        currentEffect = swordEffects[selectedEffect].InstantiateEffect(tipPoint, basePoint, transform).GetComponent<SwordEffect>();
        meshRenderer.material = currentEffect.attributes.swordMaterial;
    }

    public void StartSwing()
    {
        currentEffect.StartSwing();
    }

    public void StopSwing()
    {
        currentEffect.StopSwing();
    }
}
