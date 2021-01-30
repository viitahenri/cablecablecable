using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector2.right;
        }

        _rigidbody.MovePosition(_rigidbody.position + dir.normalized * _moveSpeed * Time.fixedDeltaTime);
    }
}
