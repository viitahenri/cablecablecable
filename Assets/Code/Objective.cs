using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    public Vector3 LinePosition { get { return _linePosition.transform.position; } }
    public bool IsActive { get { return _isActive; } }

    [SerializeField] private GameObject _linePosition;
    [SerializeField] private GameObject _light;

    [SerializeField] private UnityEvent _onActivated;
    [SerializeField] private UnityEvent _onDeactivated;

    private bool _isActive = false;
    private Collider2D _activatedCollider;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _light.SetActive(_isActive);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive)
        {
            _onActivated?.Invoke();
            _light.SetActive(true);
            _isActive = true;
            _activatedCollider = other;

            if (_audioSource != null && !_audioSource.isPlaying)
                _audioSource.Play();
        }
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (_isActive && other == _activatedCollider)
    //     {
    //         _onDeactivated?.Invoke();
    //         _light.SetActive(false);
    //         _isActive = false;
    //     }
    // }
}
