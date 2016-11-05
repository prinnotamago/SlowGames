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

    private Rigidbody _rigidBody = null;


    [System.Serializable]
    public struct SwordEnemyData
    {
        public Vector3 speed;
        public float xMoveWidth;
        public float attackMoveSpeed;
        public float waitDistance;
        public float minWaitTime;
        public float maxWaitTime;
        public float appatchDistance;
        public int maxWaitCount;
        public int generatePosNumber;
    }

    private int _waitCount = 0;
    private float _angle = 0.0f;
    private float _width = 0.1f;
    private float _playerDistance = 0.0f; //Playerまでの距離
    private float _stopPosTest = 0.0f;

    [SerializeField]
    private SwordEnemyData _data;

    void Start()
    {
        _state = new Dictionary<MoveState, Action>();
        _state.Add(MoveState.Approach, Approach);
        _state.Add(MoveState.Wait, Wait);
        _state.Add(MoveState.Attack, Attack);
        _angle = UnityEngine.Random.Range(-50.0f, 50.0f);
        _data.speed.z = UnityEngine.Random.Range(0.05f, 0.1f);
        _rigidBody = GetComponentInChildren<Rigidbody>();

        var index = UnityEngine.Random.Range(0, 101);
        if(index < 80)
        {
            _width = _data.xMoveWidth;
        }
        else
        {
            _width = 0;
        }

        //プレイヤーとエネミーの距離を止まる回数で割った距離
        var totalDistance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);
        _playerDistance = totalDistance / _data.maxWaitCount;
        _stopPosTest = UnityEngine.Random.Range(
           _playerDistance * (_data.maxWaitCount - (_waitCount + 1)),
           _playerDistance * (_data.maxWaitCount - _waitCount)
           );
    }

    /// <summary>
    /// Enemyのset関数
    /// </summary>
    /// <param name="data"></param>
    public void setState(SwordEnemyData data)
    {
        _data = data;
    }

    void Update()
    {
        _state[_moveState]();
    }

    /// <summary>
    /// 接近状態
    /// </summary>
    void Approach()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        _angle += Time.deltaTime;
        transform.Translate(Mathf.Sin(_angle * _data.speed.x) * _width, 0, _data.speed.z);
        var distance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);

        //最後の接近前
        if (distance < _data.waitDistance)
        {
            _moveState = MoveState.Wait;
            var waitTime = UnityEngine.Random.Range(_data.minWaitTime, _data.maxWaitTime);
            StartCoroutine(AttackReserve(waitTime, distance));
        }
        if (_waitCount >= _data.maxWaitCount) return;

        if (distance < _stopPosTest)
        {            
            _moveState = MoveState.Wait;
            var waitTime = UnityEngine.Random.Range(_data.minWaitTime, _data.maxWaitTime);
            StartCoroutine(AttackReserve(waitTime, distance));
            _waitCount++;

            _stopPosTest = UnityEngine.Random.Range(
           _playerDistance * (_data.maxWaitCount - (_waitCount + 1)),
           _playerDistance * (_data.maxWaitCount - _waitCount)
           );

        }
    }

    /// <summary>
    /// 待機状態
    /// </summary>
    void Wait(){}

    /// <summary>
    /// 攻撃状態
    /// </summary>
    void Attack()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < _data.appatchDistance) { return; }
        transform.Translate(0, 0, _data.attackMoveSpeed);
    }

    /// <summary>
    /// 待機中の時間管理コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackReserve(float waitTime, float distance)
    {
        var time = 0.0f;
        _rigidBody.freezeRotation = true;
        while(time < waitTime)
        {
            _rigidBody.velocity = Vector3.zero;
            time += Time.deltaTime;
            yield return null;
        }
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _width = RandomMoveWidth();
        _moveState = distance < _data.waitDistance ? MoveState.Attack : MoveState.Approach;
    }

    float RandomMoveWidth()
    {
        var index = UnityEngine.Random.Range(0, 101);
        if(index < 80)
        {
            return _data.xMoveWidth;  
        }
        else
        {
            return 0;
        }
    }

}
