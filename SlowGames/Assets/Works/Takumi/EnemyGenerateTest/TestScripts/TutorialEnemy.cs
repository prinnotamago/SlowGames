using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
public class TutorialEnemy : MonoBehaviour
{

    enum TutorialState
    {
        stay = 0,
        JustBeforeShot = 1,
        Shot = 2,
    }

    //速度,攻撃頻度等を参照する
    Enemy _enemy;

    [SerializeField]
    bool _isShot;

    //これよぶと打ちます
    public bool isShot
    {
        set { _isShot = value; }
        get { return _isShot; }
    }

    [SerializeField]
    Animator _enemyAnimator;
    [SerializeField]
    float _justShotTime = 0.0f;

    private float _shotCountTime = 0.0f; //打つまでのインターバルの時間
    [SerializeField]
    private float _minIntervalTime = 0.0f;
    [SerializeField]
    private float _maxIntervalTime = 0.0f;

    private float _intervalTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        _enemy = _enemy = this.gameObject.GetComponent<Enemy>();
        //_isShot = false;

        var player = GameObject.FindGameObjectWithTag(TagName.Player);
        transform.LookAt(player.transform.position);
        _enemyAnimator.SetInteger("TutorialState", (int)TutorialState.stay);

        _intervalTime = UnityEngine.Random.Range(_minIntervalTime, _maxIntervalTime);

        StartCoroutine(ShotMotion()); //最初に一発撃つ
    }

    void Update()
    {
        //コルーチンで管理
        if (!_isShot) return; //falseだったらTimeを図らない
        _shotCountTime += Time.deltaTime;
        if(_shotCountTime > _intervalTime)
        {
            _shotCountTime = 0;
            _intervalTime = UnityEngine.Random.Range(_minIntervalTime, _maxIntervalTime);
            StartCoroutine(ShotMotion());
        }

    }


    IEnumerator ShotMotion()
    {

        float timeCount = 0;

        //撃つ数
        int shotCount = _enemy.info.chamberValue;
        //連弾する時の遅延時間
        float shotDelayTime = _enemy.info.shotDelay;

        //アニメーションを撃つかまえに
        _enemyAnimator.SetInteger("TutorialState", (int)TutorialState.JustBeforeShot);

        //Test; wait for Secondのd代わり
        timeCount = _justShotTime;
        while (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < shotCount; i++)
        {
            //撃つ
            _enemyAnimator.SetInteger("TutorialState", (int)TutorialState.Shot);
            //shotラグ//test; wait for Second がうまく行かない代わり
            timeCount = 0.15f;

            while (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
                yield return null;
            }

            gameObject.GetComponentInChildren<EnemyShot>().DoShot();

            //二発以上かつ最後の弾じゃなければ
            if (shotCount > 1 && (shotCount - 1) > i)
            {
                //１発目以降は間隔を開けて撃つ
                timeCount = _justShotTime;
                while (timeCount > 0)
                {
                    timeCount -= Time.deltaTime;
                    yield return null;
                }
                _enemyAnimator.SetInteger("TutorialState", (int)TutorialState.JustBeforeShot);

                //shotラグ//test; wait for Second がうまく行かない代わり
                timeCount = _justShotTime;
                while (timeCount > 0)
                {
                    timeCount -= Time.deltaTime;
                    yield return null;
                }

            }

        }

        //stayに遷移
        Debug.Log("a");
        _enemyAnimator.SetInteger("TutorialState", (int)TutorialState.stay);

        yield return null;

    }

}
