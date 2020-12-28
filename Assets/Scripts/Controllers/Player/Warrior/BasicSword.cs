using UnityEngine;

public abstract class BasicSword : MonoBehaviour
{
    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator);
    public abstract string Name();
}
