using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _speed = 4;
    [SerializeField] private Vector3 _dir = Vector3.forward;

    private void Update()
    {
        Move(_dir);
        CheckEnemyStatus();
    }

    void Move(Vector3 dir)
    {
        //transform.position += new Vector3(0, 0, _speed) * Time.deltaTime;
        //transform.localPosition += dir * _speed * Time.deltaTime;
        
        transform.Translate(Vector3.forward * _speed* Time.deltaTime);
        
    }

    void CheckEnemyStatus()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, player.transform.position) > 30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Turn Right"))
        {
            int randomNum = Random.Range(0, 10);
            if (randomNum < 5)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,90,0));
            }
        }
        if (other.transform.CompareTag("Turn Left"))
        {
            int randomNum = Random.Range(0, 10);
            if (randomNum < 5)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0,90,0));
            }
        }
    }
}
