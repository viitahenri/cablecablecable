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
        Dead
    }

    enum Direction : int
    {
        Right = 0,
        Front = 1,
        Left = 2,
        Back = 3,
    }

    public UnityEvent OnDeath = new UnityEvent();

    [Header("References")]
    [SerializeField] private GameObject _lineRendererPrefab;
    [SerializeField] private TextMeshProUGUI _hudText;
    [SerializeField] private Image _sliderImage;
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _cableTransform;

    [Header("Graphics (right-front-left-back)")]
    [SerializeField] private List<Transform> _graphicsParents = new List<Transform>();

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
    private Transform _activeGraphics;
    private bool _isInit = false;
    private Vector2 _movementDirection = Vector2.zero;

    public void Init(int index)
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

        _lineRenderer.materials[1].color = Random.ColorHSV(0f, 1f, .3f, .3f, .5f, .5f);
        for (int i = 0; i < _lineRenderer.materials.Length; i++)
        {
            _lineRenderer.materials[i].renderQueue += index;
        }

        _currentState = State.Unreeling;

        _activeGraphics = _graphicsParents[(int)Direction.Front];

        _canvasTransform.gameObject.SetActive(true);

        _isInit = true;
    }

    void Update()
    {
        if (!_isInit)
            return;

        _hudText.text = $"{_maxSegmentCount - _currentLineIndex - 1}";

        if (_currentState == State.Unreeling)
        {
            // right-front-left-back
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _movementDirection += Vector2.up;
                _activeGraphics = _graphicsParents[(int)Direction.Back];
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _movementDirection += Vector2.down;
                _activeGraphics = _graphicsParents[(int)Direction.Front];
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _movementDirection += Vector2.left;
                _activeGraphics = _graphicsParents[(int)Direction.Left];
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _movementDirection += Vector2.right;
                _activeGraphics = _graphicsParents[(int)Direction.Right];
            }

            _graphicsParents.ForEach(p => p.gameObject.SetActive(p == _activeGraphics ? true : false));

            _animator.SetFloat(ANIM_WALK_SPEED_TRIGGER_NAME, (_movementDirection.normalized * _currentMoveSpeed).magnitude);

            _lineRenderer.SetPosition(_currentLineIndex, transform.position);

            var dist = Vector2.Distance(transform.position, _previousLinePosition);

            var normalized = dist / _lineSegmentLength;
            _currentMoveSpeed = _minMoveSpeed + _maxMoveSpeed * _struggleCurve.Evaluate(normalized);
            _sliderImage.fillAmount = normalized;
            _cableTransform.rotation = Quaternion.AngleAxis(5f + normalized * 355f, Vector3.forward);

            if (dist > _lineSegmentLength)
            {
                DropSegment(transform.position);
            }

            if (_lineRenderer.positionCount >= _maxSegmentCount || Input.GetKeyDown(KeyCode.K))
            {
                _currentState = State.Dead;
                Die();
            }
        }
    }

    void DropSegment(Vector3 position)
    {
        _currentLineIndex++;
        _lineRenderer.positionCount += 1;
        _lineRenderer.SetPosition(_currentLineIndex, position);
        _previousLinePosition = position;
        _onDropSegment?.Invoke();
    }

    void FixedUpdate()
    {
        if (!_isInit)
            return;

        if (_currentState == State.Unreeling)
        {
            _rigidbody.MovePosition(_rigidbody.position + _movementDirection.normalized * _currentMoveSpeed * Time.fixedDeltaTime);
            _movementDirection = Vector2.zero;
        }
    }

    void Die()
    {
        _canvasTransform.gameObject.SetActive(false);
        _rigidbody.useFullKinematicContacts = true;
        _rigidbody.isKinematic = true;
        _currentState = State.Dead;
        OnDeath?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_currentState == State.Dead)
        {
            return;
        }

        Debug.Log($"Enter {other.name}");
        var objective = other.gameObject.GetComponent<Objective>();
        if (objective)
        {
            DropSegment(objective.LinePosition);
            Die();
        }
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     var objective = other.gameObject.GetComponent<Objective>();
    //     if (objective)
    //     {

    //     }
    // }
}
