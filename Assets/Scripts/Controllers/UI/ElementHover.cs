using UnityEngine;
using UnityEngine.EventSystems;

public class ElementHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIEventSystem.current.SetHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEventSystem.current.SetHover(false);
    }
}
