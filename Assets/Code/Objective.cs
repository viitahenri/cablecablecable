using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    public Vector3 LinePosition { get { return _linePosition.transform.position; } }

    [SerializeField] private GameObject _linePosition;
    [SerializeField] private GameObject _light;

    [SerializeField] private UnityEvent _onActivated;
    [SerializeField] private UnityEvent _onDeactivated;

    private bool _isActive = false;

    void Start()
    {
        _light.SetActive(_isActive);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive)
        {
            _onActivated?.Invoke();
            _light.SetActive(true);
            _isActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (_isActive)
        {
            _onDeactivated?.Invoke();
            _light.SetActive(false);
            _isActive = false;
        }
    }
}
