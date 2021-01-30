using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spawner : MonoBehaviour
{
    
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private GameObject _robotPrefab;
    [SerializeField] private AnimationClip _openClip;
    [SerializeField] private AnimationClip _openIdleClip;
    [SerializeField] private AnimationClip _closeClip;
    [SerializeField] private SpriteRenderer _topRenderer;
    
    private Animation _animation;

    void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public IEnumerator SpawnRobotRoutine(CinemachineVirtualCamera camera, Action<GameObject> onComplete)
    {
        var layer = _topRenderer.sortingLayerName;
        var order = _topRenderer.sortingOrder;
        _topRenderer.sortingLayerName = "Robot";
        _topRenderer.sortingOrder = 999;
        yield return new WaitForEndOfFrame();

        var robotObj = Instantiate(_robotPrefab);
        robotObj.transform.position = _spawnPosition.transform.position;
        camera.Follow = robotObj.transform;

        _animation.Play(_openClip.name, PlayMode.StopAll);
        yield return new WaitForSeconds(_openClip.length);
        _animation.Play(_openIdleClip.name, PlayMode.StopAll);

        yield return new WaitForEndOfFrame();
        _topRenderer.sortingLayerName = layer;
        _topRenderer.sortingOrder = order;

        _animation.Play(_closeClip.name, PlayMode.StopAll);
        yield return new WaitForSeconds(_closeClip.length);

        onComplete?.Invoke(robotObj);
    }
}
