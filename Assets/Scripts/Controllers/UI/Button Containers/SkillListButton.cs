using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListButton : ButtonContainer
{
    public override void Clicked()
    {
        overlayControls.PickingKeyBind(buttonData.skillIndexInAdapter, button);
    }
}
