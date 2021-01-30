using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Game : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject _robotPrefab;
    [SerializeField] private GameObject _map;
    [SerializeField] private GameObject _spawnPosition;

    private Camera _mapCamera;
    private bool _mapVisible = false;
    private GameObject _currentRobot;
    private int _robotCount = 0;

    void Start()
    {
        _mapCamera = _map.GetComponent<Camera>();
        _map.SetActive(_mapVisible);
        _mapCamera.enabled = _mapVisible;

        SpawnRobot();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    void ToggleMap()
    {
        _mapVisible = !_mapVisible;
        _map.SetActive(_mapVisible);
        _mapCamera.enabled = _mapVisible;
    }

    void SpawnRobot()
    {
        var robotObj = Instantiate(_robotPrefab);
        robotObj.transform.position = _spawnPosition.transform.position;

        _currentRobot = robotObj;
        var robot = robotObj.GetComponent<Robot>();
        robot.OnDeath.AddListener(() => SpawnRobot());
        robot.Init(_robotCount++);

        _virtualCamera.Follow = robot.transform;
    }
}
