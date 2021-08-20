using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapCreater : MonoBehaviour
{
    [Header("PREFABS")]
    [SerializeField] private GameObject[] _cubePrefabs;
    [SerializeField] private List<GameObject> _legPrefabs = new List<GameObject>();
    [SerializeField] private GameObject[] _obstacles;
    [SerializeField] private GameObject _helicopter;
    
    [Header("---------------------------------")]
    [SerializeField] private GameObject _finish;
    [SerializeField] private Transform _cubeHolder;
    [SerializeField] private int _length;
    [SerializeField] private Transform _startPos;
    [SerializeField] private float _zPlus;

    private void Start()
    {
        CreateGround();
        PlacingObstacles();
    }

    void CreateGround()
    {
        _length += PlayerPrefs.GetInt("Level") / 5;
        for (int i = 0; i < _length; i++)
        {
            // SELECTING WHICH PREFAB IS INSTANTIATNG

            if (i < 3)
            {
                Instantiate(_cubePrefabs[0], _startPos.position, Quaternion.identity, _cubeHolder);
                _startPos.position += Vector3.forward*_zPlus;
            }
            else
            {
                int randomNum = Random.Range(0, 4);
                if (randomNum == 2 && _legPrefabs.Count > 0)
                {
                    Instantiate(_legPrefabs[0], _startPos.position, Quaternion.identity, _cubeHolder);
                    _legPrefabs.RemoveAt(0);
                    _startPos.position += Vector3.forward*_zPlus;
                }
                else
                {
                    Instantiate(_cubePrefabs[Random.Range(0,2)], _startPos.position, Quaternion.identity, _cubeHolder);
                    _startPos.position += Vector3.forward*_zPlus;
                }
            }
        }
        // FINISH CYCLE
        Instantiate(_finish, _startPos.position, Quaternion.identity, _cubeHolder);

    }

    void PlacingObstacles()
    {
        int totalLength = _length * 5;

        for (int i = 0; i < totalLength / 15; i++)
        {
            int randomNum = Random.Range(0, _obstacles.Length);
            if (randomNum == 2)
            {
                Instantiate(_obstacles[randomNum], new Vector3(.5f, 1, 15 * i), Quaternion.identity);
            }
            else
            {
                Instantiate(_obstacles[randomNum], new Vector3(Random.Range(0,6), 1, 15 * i), Quaternion.identity);
            }
        }
    }
}
