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

    }

    void Update()
    {
        _state[_moveState]();
    }

    private float _angle = 0.0f;
    [SerializeField]
    private float _z = -0.05f;

    [SerializeField]
    private float _waitDistance = 4.0f;

    void Approach()
    {
        transform.LookAt(Camera.main.transform);
        _angle += Time.deltaTime;
        transform.Translate(Mathf.Sin(_angle) * 0.1f, 0, _z);
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < _waitDistance)
        {
            _moveState = MoveState.Wait;
            StartCoroutine(AttackReserve());
        }
    }

    void Wait(){}

    void Attack()
    {
        transform.LookAt(Camera.main.transform);
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < 1.2f) { return; }
        transform.Translate(0, 0, 0.1f);
    }

    [SerializeField]
    private float _waitTime = 3.0f;

    IEnumerator AttackReserve()
    {
        var time = 0.0f;
        while(time < _waitTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        _moveState = MoveState.Attack;
    }

}
