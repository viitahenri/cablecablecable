using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onUnPressed;

    private bool _isPressed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isPressed)
        {
            _onPressed?.Invoke();
            _isPressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (_isPressed)
        {
            _onUnPressed?.Invoke();
            _isPressed = false;
        }
    }
}