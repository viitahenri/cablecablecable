using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Game : MonoBehaviour
{
    public int CurrentSegmentCount { get { return _currentSegmentCount; } }

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject _map;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private int _startSegmentCount = 10;
    [SerializeField] private int _segmentLengthIncrease = 10;
    [SerializeField] private List<Objective> _winningObjectives = new List<Objective>();
    [SerializeField] private GameObject _endCanvasPrefab;

    private Camera _mapCamera;
    private bool _mapVisible = false;
    private GameObject _currentRobot;
    private int _robotCount = 0;
    private int _currentSegmentCount = 0;
    private bool _wonTriggered = false;

    void Start()
    {
        _mapCamera = _map.GetComponent<Camera>();
        _map.SetActive(_mapVisible);
        _mapCamera.enabled = _mapVisible;
        _currentSegmentCount = _startSegmentCount;

        SpawnRobot();
    }

    void Update()
    {
        bool hasWon = true;
        for (int i = 0; i < _winningObjectives.Count; i++)
        {
            hasWon = _winningObjectives[i].IsActive;
        }

        if (hasWon && !_wonTriggered)
        {
            WinGame();
            _wonTriggered = true;
        }
    }

    void WinGame()
    {
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        _virtualCamera.enabled = false;
        yield return new WaitForEndOfFrame();
        var camera = Camera.main;
        var start = camera.transform.position;
        var target = transform.position + Vector3.up * 2f;
        target.z = camera.transform.position.z;
        float timer = 0f;
        while (timer <= 4f)
        {
            timer += Time.deltaTime;
            camera.transform.position = Vector3.Lerp(start, target, timer / 4f);
            yield return new WaitForEndOfFrame();
        }

        var endCanvas = Instantiate(_endCanvasPrefab).GetComponent<EndCanvas>();
        endCanvas.StartEnd();

        timer = 0f;
        while (timer <= 6f)
        {
            timer += Time.deltaTime;
            camera.transform.position = Vector3.Lerp(target, target + Vector3.up * 5f, timer / 6f);
            yield return new WaitForEndOfFrame();
        }
    }

    void ToggleMap(bool value)
    {
        _map.SetActive(value);
        _mapCamera.enabled = value;
    }

    void SpawnRobot()
    {
        if (_wonTriggered)
            return;

        ToggleMap(false);
        StartCoroutine(SpawnRobotRoutine());
    }

    IEnumerator SpawnRobotRoutine()
    {
        yield return new WaitForEndOfFrame();
        yield return _spawner.SpawnRobotRoutine(_virtualCamera, (robotObj) => 
        {
            _currentRobot = robotObj;
            var robot = robotObj.GetComponent<Robot>();
            robot.OnDeath.AddListener(() => SpawnRobot());
            robot.Init(this, _robotCount++);
            _virtualCamera.Follow = robot.transform;
        });
    }

    public void IncreaseSegmentCount()
    {
        _currentSegmentCount += _segmentLengthIncrease;
    }

    public void SetSpawner(Spawner _newSpawner)
    {
        _spawner = _newSpawner;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ToggleMap(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ToggleMap(false);
        }
    }
}
