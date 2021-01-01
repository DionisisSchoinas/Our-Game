using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickbarButton : ButtonContainer
{
    public override void Clicked()
    {
        overlayControls.SetSelectedQuickBar(buttonData.quickBarIndex);
    }
}
