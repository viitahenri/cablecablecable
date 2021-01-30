using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Robot : MonoBehaviour
{
    [SerializeField] private GameObject _lineRendererPrefab;
    [SerializeField] private TextMeshProUGUI _hudText;

    [SerializeField] private float _lineSegmentLength = 1f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _maxSegmentCount = 10;

    private Rigidbody2D _rigidbody;
    private LineRenderer _lineRenderer;
    private Vector2 _previousLinePosition;
    private int _currentLineIndex = 0;
    private bool _canMove = true;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        var line = Instantiate(_lineRendererPrefab);
        line.transform.position = Vector3.zero;
        _lineRenderer = line.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.positionCount++;
        _currentLineIndex++;
        _previousLinePosition = transform.position;
    }

    void Update()
    {
        _hudText.text = $"{_maxSegmentCount - _currentLineIndex - 1}";

        if (_lineRenderer.positionCount >= _maxSegmentCount)
        {
            _canMove = false;
        }

        if (_canMove)
        {
            _lineRenderer.SetPosition(_currentLineIndex, transform.position);

            var dist = Vector2.Distance(transform.position, _previousLinePosition);
            if (dist > _lineSegmentLength)
            {
                _currentLineIndex++;
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_currentLineIndex, transform.position);
                _previousLinePosition = transform.position;
            }
        }
    }

    void FixedUpdate()
    {
        if (_canMove)
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
}
