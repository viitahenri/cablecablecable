using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPressed;

    private bool _isPressed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isPressed)
        {
            _onPressed?.Invoke();
            _isPressed = true;
        }
    }
}