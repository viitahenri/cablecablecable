using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Game : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject _robotPrefab;

    private GameObject _currentRobot;

    void Start()
    {
        SpawnRobot();
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
