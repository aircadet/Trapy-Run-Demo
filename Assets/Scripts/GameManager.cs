using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using  UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerState playerState;
    public int _level;

    [Header("--------------------")]
    [SerializeField] private bool _infinityLevel;
    
    public enum PlayerState
    {
        Preparing,
        Playing,
        Finish,
        Fail
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // PLAYER PREFS
        _level = PlayerPrefs.GetInt("Level", 1);
        
        playerState = PlayerState.Preparing;
    }

    private void Update()
    {
        if (playerState == PlayerState.Preparing && Input.GetMouseButtonUp(0))
        {
            playerState = PlayerState.Playing;
        } 
    }

    public void NextLevel()
    {
        // FOR LIMITED LEVELS
        if (!_infinityLevel)
        {
            StartCoroutine(LimitedNextLevel());
        }
        else
        {
            StartCoroutine(InfinityNextLevel());
        }
    }

    IEnumerator InfinityNextLevel()
    {
        yield return new WaitForSeconds(.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        StartCoroutine(DelayedRestart());
    }
    
    IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LimitedNextLevel()
    {
        if (_level < SceneManager.sceneCountInBuildSettings)
        {
            yield return new WaitForSeconds(.3f);
            SceneManager.LoadScene(_level);
        }
        else
        {
            int randomNum = Random.Range(0, SceneManager.sceneCountInBuildSettings);
            yield return new WaitForSeconds(.3f);
            SceneManager.LoadScene(randomNum);
        }
        
        _level++;
        PlayerPrefs.SetInt("Level",_level);
    }
}
