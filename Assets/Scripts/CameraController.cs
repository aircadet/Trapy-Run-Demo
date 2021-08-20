using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _mainCam;
    [SerializeField] private Transform _finishCamPos;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private bool _lookAt;

    
    void Follow()
    {
        if (_lookAt)
        {
            _mainCam.transform.LookAt(_target);
        }

        _mainCam.transform.DOMove(_target.position - _offSet, .25f);
    }

    void ChangeCamAngle()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_mainCam.transform.DOMove(_finishCamPos.transform.position, 1f))
            .Join(_mainCam.transform.DORotate(_finishCamPos.transform.rotation.eulerAngles, 1));
    }
    private void Update()
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            Follow();
        }

        if (GameManager.instance.playerState == GameManager.PlayerState.Finish)
        {
            ChangeCamAngle();
        }
    }
}
