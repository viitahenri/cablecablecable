using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Game : MonoBehaviour
{
    public int CurrentSegmentCount { get { return _currentSegmentCount; } }

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject _robotPrefab;
    [SerializeField] private GameObject _map;
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private int _startSegmentCount = 10;
    [SerializeField] private int _segmentLengthIncrease = 10;

    private Camera _mapCamera;
    private bool _mapVisible = false;
    private GameObject _currentRobot;
    private int _robotCount = 0;
    private int _currentSegmentCount = 0;

    void Start()
    {
        _mapCamera = _map.GetComponent<Camera>();
        _map.SetActive(_mapVisible);
        _mapCamera.enabled = _mapVisible;
        _currentSegmentCount = _startSegmentCount;

        SpawnRobot();
    }

    void ToggleMap(bool value)
    {
        _map.SetActive(value);
        _mapCamera.enabled = value;
    }

    void SpawnRobot()
    {
        ToggleMap(false);
        StartCoroutine(SpawnRobotRoutine());
    }

    IEnumerator SpawnRobotRoutine()
    {
        yield return new WaitForEndOfFrame();
        var robotObj = Instantiate(_robotPrefab);
        robotObj.transform.position = _spawnPosition.transform.position;

        _currentRobot = robotObj;
        var robot = robotObj.GetComponent<Robot>();
        robot.OnDeath.AddListener(() => SpawnRobot());
        robot.Init(this, _robotCount++);

        _virtualCamera.Follow = robot.transform;
    }

    public void IncreaseSegmentCount()
    {
        _currentSegmentCount += _segmentLengthIncrease;
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
