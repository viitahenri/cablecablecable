using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onUnPressed;

    private bool _isPressed = false;
    private bool _isDone = false;

    public void SetDone()
    {
        _isDone = true;
    }

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
        if (_isPressed && !_isDone)
        {
            var robot = other.attachedRigidbody.GetComponent<Robot>();
            if (!robot.IsAlive())
                return;
            _onUnPressed?.Invoke();
            _isPressed = false;
        }
    }
}