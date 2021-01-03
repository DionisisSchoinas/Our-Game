using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public int uniqueOverlayToWeaponAdapterId;

    public float cooldown = 0.5f;

    public bool onCooldown = false;

    public abstract string Type { get; }

    public abstract string Name { get; }

    public void StartCooldown()
    {

    }

    private IEnumerator StartCooling()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    } 
}
