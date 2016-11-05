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

    private float _speed = 0.0f;

    private float _angle = 0.0f;

    private float _xMoveWidth = 0.1f;

    private float _attackMoveSpeed = 0.1f;

    [SerializeField]
    private float _waitDistance = 4.0f;

    [SerializeField]
    private float _waitTime = 3.0f;

    [SerializeField]
    private float _appatchDistance = 1.2f;

    void Start()
    {
        _state = new Dictionary<MoveState, Action>();
        _state.Add(MoveState.Approach, Approach);
        _state.Add(MoveState.Wait, Wait);
        _state.Add(MoveState.Attack, Attack);
        _angle = UnityEngine.Random.Range(-50.0f, 50.0f);
        _speed = UnityEngine.Random.Range(0.05f, 0.1f);
        _rigidBody = GetComponentInChildren<Rigidbody>();
    }

    /// <summary>
    /// EnemyをWaveごとに設定を変えるために、
    /// 生成されたときに呼ぶ関数
    /// </summary>
    /// <param name="speed">移動スピード</param>
    /// <param name="xMoveWidth">横の移動幅(0.1～0.5くらいが良い)</param>
    /// <param name="waitDistance">待機したときのプレイヤーとの距離</param>
    /// <param name="waitTime">待機時間</param>
    /// <param name="attackMoveSpeed">攻撃モーション時の向かってくる移動スピード</param>
    public void setState(float speed, float xMoveWidth, float waitDistance, float waitTime, float attackMoveSpeed)
    {
        _speed = speed;
        _xMoveWidth = xMoveWidth;
        _waitDistance = waitDistance;
        _waitTime = waitTime;
        _attackMoveSpeed = attackMoveSpeed;
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
        transform.Translate(Mathf.Sin(_angle) * _xMoveWidth, 0, _speed);
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < _waitDistance)
        {
            _moveState = MoveState.Wait;
            StartCoroutine(AttackReserve());
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
        if(distance < _appatchDistance) { return; }
        transform.Translate(0, 0, _attackMoveSpeed);
    }

    /// <summary>
    /// 待機中の時間管理コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackReserve()
    {
        var time = 0.0f;
        _rigidBody.freezeRotation = true;
        while(time < _waitTime)
        {
            _rigidBody.velocity = Vector3.zero;
            time += Time.deltaTime;
            yield return null;
        }
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _moveState = MoveState.Attack;
    }

}
