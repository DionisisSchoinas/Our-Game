using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector]
    public int uniqueOverlayToWeaponAdapterId;
    [HideInInspector]
    public bool onCooldown;

    public abstract string type { get; }

    public abstract string skillName { get; }

    public abstract float cooldown { get; }

    private void Awake()
    {
        onCooldown = false;
    }

    public void StartCooldown()
    {
        onCooldown = true;
        Invoke(nameof(CooledDown), cooldown);
    }

    private void CooledDown()
    {
        onCooldown = false;
    } 
}
