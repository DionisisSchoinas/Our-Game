﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDodge : Skill
{
    [SerializeField]
    private float dodgeDuration = 0.2f;

    public override string type => "Dodge";
    public override string skillName => "Roll";
    public override float cooldown => 3f;
    public override float duration => dodgeDuration;
    public override float instaCastDelay => 0f;
    public override bool instaCast => false;
    public override float manaCost => 0f;

    public new void StartCooldown()
    {
        StartCooldownWithoutEvent(cooldown);
    }
}
