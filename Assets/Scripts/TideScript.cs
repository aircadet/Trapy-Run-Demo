using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  DG.Tweening;

public class TideScript : MonoBehaviour
{
    [SerializeField] private float tideAmount, time;
    private Vector3 firstPos;

    private void Start()
    {
        firstPos = transform.position;
        Tide();
    }
    
    void Tide()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(firstPos + (tideAmount * Vector3.right), time))
            .Append(transform.DOMove(firstPos - (tideAmount * Vector3.right), 2*time))
            .SetLoops(-1);
    }
}
