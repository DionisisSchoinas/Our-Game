﻿using UnityEngine;

public abstract class BasicSword : MonoBehaviour
{
    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh);
    public abstract string Name();
}
