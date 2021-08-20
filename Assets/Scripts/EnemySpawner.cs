using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("ENEMY Settings")]
    [SerializeField] private GameObject _basicEnemy;
    [SerializeField] private GameObject _smartEnemy;
    [SerializeField] private Transform _enemyHolder;
    [SerializeField] private float _waitForSpawn;

    [Header("Spawning Settings")] 
    [SerializeField] private bool _infinitySpawn = false;
    [SerializeField] private int _spawnCount;
    [SerializeField] private float _spawnDistance;

    private bool isSpawned = false;

    [Header("Following Settings")] 
    [SerializeField] private bool _follow;
    [SerializeField] private Transform _target;
    [SerializeField] private float _followDistance = 4;


    private bool wait =false;
    private void Update()
    {
        // FOLLOW 
        if (_follow)
        {
            Follow();
        }

        if (GameManager.instance.playerState == GameManager.PlayerState.Playing && !wait && _infinitySpawn)
        {
            StartCoroutine(SpawnEnemyInfinity(_waitForSpawn));
        }
        
        // SPAWN FOR COUNT

        if (!_infinitySpawn && _spawnCount > 0 && !isSpawned && Vector3.Distance(transform.position,GameObject.Find("Player").transform.position) < _spawnDistance)
        {
            if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
            {
                SpawnEnemy();
            }
        }
    }

    void Follow()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _target.position.z - _followDistance);
        transform.position = pos;
    }

    
    void SpawnEnemy()
    {
        isSpawned = true;
        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(transform.position.x - 2, transform.position.x + 2), transform.position.y, transform.position.z);
            Instantiate(_basicEnemy, pos,transform.rotation, _enemyHolder);
        }
    }

    // FOR INFINITY SPAWN
    IEnumerator SpawnEnemyInfinity(float time)
    {
        wait = true;

        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = new Vector3(Random.Range(transform.position.x - 2, transform.position.x + 2), transform.position.y, transform.position.z);
            Instantiate(_basicEnemy, pos, Quaternion.identity, _enemyHolder);
        }

        yield return new WaitForSeconds(time);
        wait = false;

    }
}
