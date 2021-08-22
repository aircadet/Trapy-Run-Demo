using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _speed = 4, _minSpeed, _maxSpeed;
    [SerializeField] private Vector3 _dir = Vector3.forward;
    [SerializeField] private Transform _jumpHolder;
    
    Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        Move(_dir);
        CheckEnemyStatus();
        CalculateEnemySpeed();
    }

    void Move(Vector3 dir)
    {
        if (GameManager.instance.playerState != GameManager.PlayerState.Preparing)
        {
            transform.Translate( _speed* Time.deltaTime * Vector3.forward);
        }
    }

    void CalculateEnemySpeed()
    {
        _speed = Vector3.Distance(transform.position,
            new Vector3(transform.position.x, transform.position.y, player.position.z));

        if (_speed <= _minSpeed)
        {
            _speed = _minSpeed;
        }
        else if (_speed > _maxSpeed)
        {
            _speed = _maxSpeed;
        }
    }

    void CheckEnemyStatus()
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 30 || transform.position.y < -10 )
            {
                float randomX, randomZ;
                randomX = Random.Range(0, 8.8f);
                randomZ = Random.Range(4f, 6f);
                transform.position = new Vector3(randomX, player.position.y, player.position.z - randomZ);
                transform.rotation = Quaternion.identity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag("Turnable Enemy"))
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
        else if (other.transform.CompareTag("Fallen Player") || other.transform.CompareTag("Fallen Enemy"))
        {
            JumpToPlayer(other.gameObject);
        }
    }

    void JumpToPlayer(GameObject other)
    {
        // CHANGE TAG
        transform.tag = "Fallen Enemy";
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<EnemyController>());
        
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("Death", 1);
        Vector3 pos;

        pos = other.transform.position;

        transform.parent = _jumpHolder;
        transform.DOJump(pos, .5f, 1, 1f);
    }
}
