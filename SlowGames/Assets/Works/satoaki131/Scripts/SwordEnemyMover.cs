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
        public float speed;
        public float angle;
        public float xMoveWidth;
        public float attackMoveSpeed;
        public float waitDistance;
        public float waitTime;
        public float appatchDistance;
    }

    private SwordEnemyData _data;

    void Start()
    {
        _state = new Dictionary<MoveState, Action>();
        _state.Add(MoveState.Approach, Approach);
        _state.Add(MoveState.Wait, Wait);
        _state.Add(MoveState.Attack, Attack);
        _data.angle = UnityEngine.Random.Range(-50.0f, 50.0f);
        _data.speed = UnityEngine.Random.Range(0.05f, 0.1f);
        _rigidBody = GetComponentInChildren<Rigidbody>();
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
        _data.angle += Time.deltaTime;
        transform.Translate(Mathf.Sin(_data.angle) * _data.xMoveWidth, 0, _data.speed);
        var distance = transform.localPosition.z - Camera.main.transform.localPosition.z;
        if(distance < _data.waitDistance)
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
        if(distance < _data.appatchDistance) { return; }
        transform.Translate(0, 0, _data.attackMoveSpeed);
    }

    /// <summary>
    /// 待機中の時間管理コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackReserve()
    {
        var time = 0.0f;
        _rigidBody.freezeRotation = true;
        while(time < _data.waitTime)
        {
            _rigidBody.velocity = Vector3.zero;
            time += Time.deltaTime;
            yield return null;
        }
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _moveState = MoveState.Attack;
    }

}
