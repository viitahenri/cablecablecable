using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    
    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Count)];
    }
}
