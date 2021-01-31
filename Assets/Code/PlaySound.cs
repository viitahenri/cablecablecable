using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _audioClips = new List<AudioClip>();

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (_audioClips.Count > 0)
            _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Count)]);
    }

    public void PlayByName(string name)
    {
        var found = _audioClips.Find(a => a.name == name);
        if (found)
        {
            _audioSource.PlayOneShot(found);
        }
    }
}
