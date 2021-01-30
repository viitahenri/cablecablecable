using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _closedGameObject;
    [SerializeField] private GameObject _openGameObject;


    public void Open()
    {
        _closedGameObject.SetActive(false);
    }
}
