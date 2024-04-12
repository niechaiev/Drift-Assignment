using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressReleaseHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Action OnPressed;
    public Action OnReleased;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleased?.Invoke();
    }
}