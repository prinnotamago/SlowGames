using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordEnemyMover : MonoBehaviour
{
    public enum MoveState
    {
        Approach,
        Wait,
        Attack
    }

    private Dictionary<MoveState, Action> _state = null;

    private MoveState _moveState = MoveState.Approach;

    void Start()
    {
        _state = new Dictionary<MoveState, Action>();
        _state.Add(MoveState.Approach, Approach);
        _state.Add(MoveState.Wait, Wait);
        _state.Add(MoveState.Attack, Attack);
        _angle = UnityEngine.Random.Range(-50.0f, 50.0f);
        _speed = UnityEngine.Random.Range(_zSpeedMin, _zSpeedMax);
    }

    void Update()
    {
        _state[_moveState]();
    }

    private float _speed = 0.0f;

    private float _angle = 0.0f;
    [SerializeField]
    private float _zSpeedMin = 0.05f;
    [SerializeField]
    private float _zSpeedMax = 0.1f;

    [SerializeField]
    private float _waitDistance = 4.0f;

    void Approach()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        _angle += Time.deltaTime;
        transform.Translate(Mathf.Sin(_angle) * 0.1f, 0, _speed);
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < _waitDistance)
        {
            _moveState = MoveState.Wait;
            StartCoroutine(AttackReserve());
        }
    }

    void Wait(){}

    [SerializeField]
    private float _appatchDistance = 1.2f;

    void Attack()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < _appatchDistance) { return; }
        transform.Translate(0, 0, 0.1f);
    }

    [SerializeField]
    private float _waitTime = 3.0f;

    IEnumerator AttackReserve()
    {
        var time = 0.0f;
        GetComponentInChildren<Rigidbody>().freezeRotation = true;
        while(time < _waitTime)
        {
            GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            Debug.Log(GetComponentInChildren<Rigidbody>().velocity);
            time += Time.deltaTime;
            yield return null;
        }
        GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _moveState = MoveState.Attack;
    }

}
