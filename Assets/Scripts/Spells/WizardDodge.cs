using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardDodge : Skill
{
    public override string type => "Dodge";

    public override string skillName => "Dash";

    public override float cooldown => 3f;

    public new void StartCooldown()
    {
        StartCooldownWithoutEvent(cooldown);
    }
}
