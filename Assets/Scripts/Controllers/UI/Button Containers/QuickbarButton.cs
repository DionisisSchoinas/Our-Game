using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickbarButton : ButtonContainer
{
    public override void Clicked()
    {
        overlayControls.SetSelectedQuickBar(buttonData.quickBarIndex);
    }
    
}
