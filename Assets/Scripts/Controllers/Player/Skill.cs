using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector]
    public int uniqueOverlayToWeaponAdapterId;
    [HideInInspector]
    public bool onCooldown = false;

    public abstract string type { get; }

    public abstract string skillName { get; }

    public abstract float cooldown { get; }

    public void StartCooldown()
    {
        onCooldown = true;
        Debug.Log(cooldown);
        Invoke(nameof(CooledDown), cooldown);
    }

    private void CooledDown()
    {
        onCooldown = false;
    } 
}
