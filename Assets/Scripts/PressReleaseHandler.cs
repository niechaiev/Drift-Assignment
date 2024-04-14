using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressReleaseHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{    
    [SerializeField] private Color pressed;
    [SerializeField] private Color released;
    public Action OnPressed;
    public Action OnReleased;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed?.Invoke();
        _image.color = pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleased?.Invoke();
        _image.color = released;
    }
}