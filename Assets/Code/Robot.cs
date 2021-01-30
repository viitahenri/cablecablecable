using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Robot : MonoBehaviour
{
    private const string ANIM_WALK_SPEED_TRIGGER_NAME = "WalkSpeed";

    enum State
    {
        Unreeling,
        Struggle
    }

    public UnityEvent OnDeath = new UnityEvent();

    [Header("References")]
    [SerializeField] private GameObject _lineRendererPrefab;
    [SerializeField] private TextMeshProUGUI _hudText;
    [SerializeField] private Image _sliderImage;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _cableTransform;

    [Header("Gameplay properties")]
    [SerializeField] private float _lineSegmentLength = 1f;
    [SerializeField] private float _minMoveSpeed = .2f;
    [SerializeField] private float _maxMoveSpeed = 5f;
    [SerializeField] private AnimationCurve _struggleCurve = new AnimationCurve();
    [SerializeField] private int _maxSegmentCount = 10;

    [SerializeField] private UnityEvent _onDropSegment = new UnityEvent();

    private Rigidbody2D _rigidbody;
    private LineRenderer _lineRenderer;
    private Vector2 _previousLinePosition;
    private int _currentLineIndex = 0;
    private State _currentState;
    private float _currentMoveSpeed;

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

        _currentState = State.Unreeling;
    }

    void Update()
    {
        _hudText.text = $"{_maxSegmentCount - _currentLineIndex - 1}";

        if (_currentState == State.Unreeling)
        {
            if (_lineRenderer.positionCount >= _maxSegmentCount)
            {
                _currentState = State.Struggle;
                OnDeath?.Invoke();
            }

            _lineRenderer.SetPosition(_currentLineIndex, transform.position);

            var dist = Vector2.Distance(transform.position, _previousLinePosition);

            var normalized = dist / _lineSegmentLength;
            _currentMoveSpeed = _minMoveSpeed + _maxMoveSpeed * _struggleCurve.Evaluate(normalized);
            _sliderImage.fillAmount = normalized;
            _cableTransform.rotation = Quaternion.AngleAxis(normalized * 360f, Vector3.forward);

            if (dist > _lineSegmentLength)
            {
                _currentLineIndex++;
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_currentLineIndex, transform.position);
                _previousLinePosition = transform.position;

                _onDropSegment?.Invoke();
            }
        }
    }

    void FixedUpdate()
    {
        if (_currentState == State.Unreeling)
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

            _rigidbody.MovePosition(_rigidbody.position + dir.normalized * _currentMoveSpeed * Time.fixedDeltaTime);

            _animator.SetFloat(ANIM_WALK_SPEED_TRIGGER_NAME, (dir.normalized * _currentMoveSpeed).magnitude);
        }
    }
}
