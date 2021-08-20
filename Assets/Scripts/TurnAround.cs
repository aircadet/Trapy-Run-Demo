using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour
{
    [SerializeField] Vector3 _turnDir;
    [SerializeField] private float _turnSpeed;
    private void Update()
    {
        _turnDir += Vector3.up * _turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(_turnDir);
    }
}
