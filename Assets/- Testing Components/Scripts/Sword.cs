using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject swordObject;
    public Transform swordMotionRoot;
    public Transform tipPoint;
    public Transform basePoint;
    public SwordEffect[] swordEffects;

    public float delayBeforeSwing;
    public float delayBeforeStoppingSwing;

    private int selectedEffect;
    private SwordEffect currentEffect;
    private Renderer swordRenderer;

    private void Start()
    {
        swordRenderer = swordObject.GetComponent<SkinnedMeshRenderer>();
        if (swordRenderer == null)
            swordRenderer = swordObject.GetComponent<MeshRenderer>();

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
        currentEffect = swordEffects[selectedEffect].InstantiateEffect(tipPoint, basePoint, swordMotionRoot).GetComponent<SwordEffect>();
        currentEffect.transform.position = swordObject.transform.position;
        currentEffect.transform.rotation = swordObject.transform.rotation;

        swordRenderer.material = currentEffect.attributes.swordMaterial;
    }

    public void StartSwing()
    {
        StartCoroutine(DelayBeforeTrail());
    }

    private IEnumerator DelayBeforeTrail()
    {
        Debug.Log("Swing");
        yield return new WaitForSeconds(delayBeforeSwing);
        currentEffect.StartSwing();
        yield return new WaitForSeconds(delayBeforeStoppingSwing);
        StopSwing();
    }

    public void StopSwing()
    {
        currentEffect.StopSwing();
    }
}
