using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Game : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject _robotPrefab;
    [SerializeField] private GameObject _map;

    private Camera _mapCamera;
    private bool _mapVisible = false;
    private GameObject _currentRobot;

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
        var robot = Instantiate(_robotPrefab);
        robot.transform.position = Vector3.zero;

        _currentRobot = robot;
        robot.GetComponent<Robot>().OnDeath.AddListener(() => SpawnRobot());

        _virtualCamera.Follow = robot.transform;
    }
}
