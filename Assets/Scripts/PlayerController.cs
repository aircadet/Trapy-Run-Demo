using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using  DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")] [SerializeField]
    private DynamicJoystick _joystick;

    [SerializeField] private float _forwardSpeed, _sideSpeed, _turnAngle;
    [SerializeField] private Transform _helicopterPoint;

    private void Start()
    {
        if (_helicopterPoint == null)
        {
            _helicopterPoint = GameObject.FindWithTag("Helicopter Point").transform;
        }
    }

    private void Update()
    {
        CheckPlayerState();
    }

    void CheckPlayerState()
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            Move();
        }

        if (GameManager.instance.playerState == GameManager.PlayerState.Playing && transform.position.y < -10)
        {
            GameManager.instance.playerState = GameManager.PlayerState.Fail;
        }
    }

    void Move()
    {
        // MOVE FORWARD ALLWAYS
        transform.position += Vector3.forward * _forwardSpeed * Time.deltaTime;

        // SIDE MOVING
        float _turn = _joystick.Horizontal;
        if (_turn > 0)
        {
            transform.position += Vector3.right * _sideSpeed * Time.deltaTime;
            transform.DORotate(new Vector3(0, _turnAngle * _turn, 0), .5f);
        }
        else if (_turn < 0)
        {
            transform.position -= Vector3.right * _sideSpeed * Time.deltaTime;
            transform.DORotate(new Vector3(0, _turnAngle * _turn, 0), .5f);
        }
        else
        {
            transform.DORotate(Vector3.zero, .5f);
        }
    }

    void MoveToHelicopter(Transform finish)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(finish.position, .5f))
            .Append(transform.DOJump(_helicopterPoint.position, 2, 1, 1))
            .Append(_helicopterPoint.parent.transform.DORotate(new Vector3(10,90,0),.5f))
            .Append(_helicopterPoint.parent.transform.DOMove(_helicopterPoint.parent.position + new Vector3(30, 0, 0),
                5f));

        transform.parent = _helicopterPoint.parent.transform;
        Destroy(GetComponent<Rigidbody>());
    }

    private void OnCollisionEnter(Collision other)
    {

        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            if (other.transform.CompareTag("Finish"))
            {
                ParticleSystem confetti = GameObject.FindWithTag("Confetti").gameObject.GetComponent<ParticleSystem>();
                confetti.Play();
                GameManager.instance.playerState = GameManager.PlayerState.Finish;
                MoveToHelicopter(other.transform);

            }
            else if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Turnable Enemy"))
            {
                // DEATH
                transform.tag = "Fallen Player";
                Destroy(GetComponent<Rigidbody>());

                GetComponent<Animator>().SetInteger("Death", 1);
                GameManager.instance.playerState = GameManager.PlayerState.Fail;
            }
            else if (other.transform.CompareTag("Ground"))
            {
                ChangeColorSmoothly(other.gameObject.transform.GetChild(1).gameObject);
            }
            else if (other.transform.CompareTag("Obstacle"))
            {
                AddForce(other.transform.position);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            BreakCube(other.gameObject);
        }
        
        
    }

    void BreakCube(GameObject cube)
    {
        cube.AddComponent<Rigidbody>();
        cube.transform.localScale = new Vector3(.96f, .96f, .96f);
        
        Destroy(cube,2);
    }

    void ChangeColorSmoothly(GameObject cube)
    {
        cube.GetComponent<MeshRenderer>().material.DOColor(Color.red, 1.5f);
    }
    
    void AddForce(Vector3 hittedObj)
    {
        Vector3 direction = transform.position - hittedObj;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {

            if (direction.x > 0)
            {
                // OBJ HIT RIGHT SIDE -> ADD FORCE TO LEFT SIDE
                GetComponent<Rigidbody>().AddForce(Vector3.left * 20, ForceMode.Impulse);
            }
            else
            {
                // OBJ HIT LEFT SIDE -> ADD FORCE TO RIGHT SIDE
                GetComponent<Rigidbody>().AddForce(Vector3.right * 20, ForceMode.Impulse);
            }
        }
        else
        {

            if (direction.z > 0)
            {
                // OBJ HIT BACK SIDE -> ADD FORCE TO FRONT SIDE
                GetComponent<Rigidbody>().AddForce(Vector3.forward * 20, ForceMode.Impulse);
            }
            else
            {
                // OBJ HIT FRONT SIDE -> ADD FORCE TO BACK SIDE
                GetComponent<Rigidbody>().AddForce(Vector3.back * 20, ForceMode.Impulse);
            }
 
        }
    }
}
