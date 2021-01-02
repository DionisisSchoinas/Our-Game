using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickbarButton : ButtonContainer, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        overlayControls.SetSelectedQuickBar(buttonData.quickBarIndex);
    }
}
