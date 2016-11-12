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
        AttackMove,
        Attack
    }

    private Dictionary<MoveState, Action> _state = null;

    private MoveState _moveState = MoveState.Approach;

    private Rigidbody _rigidBody = null;

    [SerializeField, Tooltip("当たった時のジェネレーター")]
    RagdollGenerator _generator;

    [System.Serializable]
    public struct SwordEnemyData
    {
        public SlashSword.SlashPattern enemyPattern;
        public Vector3 minSpeed;
        public Vector3 maxSpeed;
        public float attackMoveSpeed;
        public float X_MOVE_WIDTH;
        public float waitDistance;
        public float minWaitTime;
        public float maxWaitTime;
        public float lastMinWaitTime;
        public float lastMaxWaitTime;
        public float appatchDistance;
        [Range(0, 100), Tooltip("くねくね移動の確立(％)")]
        public int meanderingPercent;
        public int generatePosNumber;
    }

    private int _waitCount = 0;
    private int _maxWaitCount = 0;
    private float _playerDistance = 0.0f; //Playerまでの距離
    private float _stopPosTest = 0.0f;
    private float _waitDirection = 0.0f;
    private Vector3 _speed;

    [SerializeField]
    private SwordEnemyData _data;
    public SwordEnemyData data
    {
        get { return _data; }
    }

    private bool isHit = false;

    private bool _isCreate = false;

    private int _direction = -1;

    ////////////////////////////////////////////////////////test//////////////////////////////////////////

    public enum AnimationState
    {
        Run = 0,
        Right,
        Left,
        Wait,
        Attack
    }

    private Animator _animator = null;
    private AnimationState _animState = AnimationState.Run;
    ///////////////////////////////////////////////////////testEnd////////////////////////////////////////


    void Start()
    {
        _animator = GetComponent<Animator>(); //test
        _state = new Dictionary<MoveState, Action>();
        _state.Add(MoveState.Approach, Approach);
        _state.Add(MoveState.Wait, Wait);
        _state.Add(MoveState.AttackMove, AttackMove);
        _state.Add(MoveState.Attack, Attack);
        _speed.z = UnityEngine.Random.Range(_data.minSpeed.z, _data.maxSpeed.z);
        _speed.x = UnityEngine.Random.Range(_data.minSpeed.x, _data.maxSpeed.x);
        _rigidBody = GetComponent<Rigidbody>();
        _direction = UnityEngine.Random.Range(-1, 2);
        _maxWaitCount = UnityEngine.Random.Range(0, 3);
        while (_direction == 0)
        {
            _direction = UnityEngine.Random.Range(-1, 2);
        }

        //プレイヤーとエネミーの距離
        var totalDistance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);
        //プレイヤーとエネミーの距離を止まる回数で割った距離
        _playerDistance = totalDistance / _maxWaitCount;
        //最初に止まるときの場所を決める
        _stopPosTest = UnityEngine.Random.Range(
           _playerDistance * (_maxWaitCount - (_waitCount + 1)),
           _playerDistance * (_maxWaitCount - _waitCount)
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

    private float _moveWidth = 0.0f; //移動した幅を保存する
    /// <summary>
    /// 接近状態
    /// </summary>
    void Approach()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        var movePos = new Vector3(_direction * _speed.x, 0, _speed.z);
        _moveWidth += movePos.x;
        if (_moveWidth > _data.X_MOVE_WIDTH)
        {
            _moveWidth = _data.X_MOVE_WIDTH;
            _direction *= -1;
            _animState = AnimationState.Left;
            _animator.SetInteger("motion", (int)_animState);
        }
        else if (_moveWidth < -_data.X_MOVE_WIDTH)
        {
            _moveWidth = -_data.X_MOVE_WIDTH;
            _direction *= -1;
            _animState = AnimationState.Right;
            _animator.SetInteger("motion", (int)_animState);
        }
        transform.Translate(movePos * Time.deltaTime);

        var distance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);
        //最後の接近前
        if (distance < _data.waitDistance)
        {
            _moveState = MoveState.Wait;
            var waitTime = UnityEngine.Random.Range(_data.lastMinWaitTime, _data.lastMaxWaitTime);
            StartCoroutine(AttackReserve(waitTime, distance));
        }
        //止まる回数が最大まで行ったら下は通らない
        if (_waitCount >= _maxWaitCount) return;

        //最後の接近じゃなくて指定した距離まで進んだらWait状態
        if (distance < _stopPosTest)
        {
            _moveState = MoveState.Wait;
            var waitTime = UnityEngine.Random.Range(_data.minWaitTime, _data.maxWaitTime);
            StartCoroutine(AttackReserve(waitTime, distance));
            _waitCount++;

            _stopPosTest = UnityEngine.Random.Range(
           _playerDistance * (_maxWaitCount - (_waitCount + 1)),
           _playerDistance * (_maxWaitCount - _waitCount)
           );

        }
    }

    /// <summary>
    /// 待機状態
    /// </summary>
    void Wait()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        var movePos = new Vector3(_waitDirection, 0, 0);
        _moveWidth += movePos.x;
        if (_moveWidth > _data.X_MOVE_WIDTH)
        {
            _moveWidth = _data.X_MOVE_WIDTH;
            _direction *= -1;
            _animState = AnimationState.Left;
            _animator.SetInteger("motion", (int)_animState);
        }
        else if (_moveWidth < -_data.X_MOVE_WIDTH)
        {
            _moveWidth = -_data.X_MOVE_WIDTH;
            _direction *= -1;
            _animState = AnimationState.Right;
            _animator.SetInteger("motion", (int)_animState);
        }

        transform.Translate(movePos * Time.deltaTime);
    }

    /// <summary>
    /// 攻撃前の移動状態
    /// </summary>
    void AttackMove()
    {
        transform.LookAt(new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z));
        var distance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);
        if (distance > _data.appatchDistance)
        {
            var movePos = new Vector3(0, 0, _data.attackMoveSpeed);
            transform.Translate(movePos * Time.deltaTime);
        }
        else
        {
            Debug.Log("AttackStart");
            _moveState = MoveState.Attack;
            StartCoroutine(AttackAnimStart());
        }
    }

    private IEnumerator AttackAnimStart()
    {
        _animState = AnimationState.Attack;
        _animator.SetTrigger("test");

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }
        var distance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);

        while (distance < _data.waitDistance)
        {
            distance = Vector3.Distance(transform.localPosition, Camera.main.transform.localPosition);
            var movePos = new Vector3(_direction * _speed.x, 0, -_speed.z);
            transform.Translate(movePos * Time.deltaTime);
            yield return null;
        }

        _moveState = MoveState.Wait;
        var waitTime = UnityEngine.Random.Range(_data.lastMinWaitTime, _data.lastMaxWaitTime);
        StartCoroutine(AttackReserve(waitTime, distance));

    }

    void Attack()
    {

    }

    private float _moveDirection = 0.5f;

    /// <summary>
    /// 待機中の時間管理コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackReserve(float waitTime, float distance)
    {
        var time = 0.0f;
        //RigidBodyのRotationを固定する
        _rigidBody.freezeRotation = true;

        //wait中のじりじり動く方向を決める
        _waitDirection = UnityEngine.Random.Range(-1, 2);
        _animState = _waitDirection == -1 ? AnimationState.Right : _waitDirection == 0 ? AnimationState.Run/*Waitに変える*/ : AnimationState.Left;
        _animator.SetInteger("motion", (int)_animState);
        _waitDirection *= _moveDirection;


        //一定時間止まる
        while (time < waitTime)
        {
            //エネミー同市でぶつかって飛んでいかないように
            _rigidBody.velocity = Vector3.zero;
            time += Time.deltaTime;
            yield return null;
        }
        //RigidBodyのRotationを動いてた時のに戻す
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //くねくねするか直線で進むか決める
        _direction = RandomMoveWidth();
        //接近モード、もしくはアタックモードに移行
        _moveState = distance < _data.waitDistance ? MoveState.AttackMove : MoveState.Approach;
        if (_moveState != MoveState.AttackMove) yield break;
        _animState = AnimationState.Run; //後でアタックモーションに変える
        _animator.SetInteger("motion", (int)_animState);

    }

    /// <summary>
    /// くねくね動くか直線で動くかを決める関数
    /// </summary>
    /// <returns></returns>
    int RandomMoveWidth()
    {
        var index = UnityEngine.Random.Range(0, 101);
        if (index < _data.meanderingPercent)
        {
            _animState = _direction == -1 ? AnimationState.Left : AnimationState.Right;
            _animator.SetInteger("motion", (int)_animState);
            return _direction;
        }
        else
        {
            _animState = AnimationState.Run;
            _animator.SetInteger("motion", (int)_animState);
            return 0;
        }
    }

    //以下あたり判定/////////////////////////////////////////////////////////////////////////////////////////

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == TagName.Sword)
        {
            if (!isHit) { return; }
            if (!col.GetComponent<SlashSword>().IsAttack) { return; }
            if(_data.enemyPattern != col.GetComponent<SlashSword>().pattern) { return; }

            //var obj = GetComponentInChildren<Rigidbody>();

            //var length = (transform.position - col.transform.position).normalized;
            //GetComponent<Rigidbody>().velocity = (length + (Vector3.up * 1.0f)) * 10.0f;

            if (_isCreate) { return; }
            else { _isCreate = true; }

            _generator.Generate(transform, GetComponent<Rigidbody>().velocity);

            //自分がどこで生成されていたかを死に際に渡す
            FindObjectOfType<GenerateSwordEnemy>().UpdateEnemyCount(_data.generatePosNumber);
            //Score加算
            ScoreManager.instance.AddScore(_data.enemyPattern);
            ScoreManager.instance.AddHitEnemyCount();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == TagName.MainCamera)
        {
            isHit = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == TagName.MainCamera)
        {
            isHit = false;
        }
    }

}
