using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    /// <summary>
    /// ボスの体力
    /// </summary>
    [SerializeField]
    int _hp = 10;

    /// <summary>
    /// ボスの形態が変わるHP
    /// </summary>
    [SerializeField]
    int[] _changeStateHP;
    int _changeStateIndex = 0;  // 形態が変わるごとに1つ進める  

    /// <summary>
    /// パージするパーツ
    /// </summary>
    [SerializeField]
    BossPurgeParts[] _parts;

    /// <summary>
    /// パージするHP
    /// </summary>
    [SerializeField]
    int[] _purgeHP;
    int _purgeIndex = 0;    // 1つパージするごとに index を1つ進める

    /// <summary>
    /// ボスの機銃の位置
    /// </summary>
    [SerializeField]
    Transform _frontGunObjPos;
    [SerializeField]
    Transform[] _sideGunObjPos;

    /// <summary>
    /// ボスの撃つ弾
    /// </summary>
    [SerializeField]
    GameObject _normalBullet;   // 直線
    [SerializeField]
    GameObject _homingBullet;   // 誘導
    [SerializeField]
    GameObject _speedBullet;    // 速い弾

    /// <summary>
    /// 速い球を撃つHP
    /// </summary>
    [SerializeField]
    int[] _speedBulletHP;
    int _speedBulletIndex = 0;  // 速い弾を撃つごとに１つ進める
    bool _speedBulletFlag = false;  // 速い弾を撃つフラグ
    [SerializeField]
    float _speedBulletChargeMax = 2.0f;
    float _speedBulletChargeTime = 0.0f;

    /// <summary>
    /// 攻撃頻度(秒)
    /// </summary>
    [SerializeField]
    float _attackInterval = 1.0f;  // 間隔
    float _attackTime = 0.0f;      // 何秒か数える
    [SerializeField, Range(0.0f, 1.0f)]
    float _patternRatio = 0.5f;           // 攻撃パターンの比率

    /// <summary>
    /// プレイヤーの位置を知るのに使う
    /// </summary>
    GameObject _player = null;

    enum BossState
    {
        START,
        LEVEL_1,
        LEVEL_2,
        LAST,
    }
    [SerializeField]
    BossState _state = BossState.START;

    [SerializeField]
    Animator _anim = null;
    bool _stateChangeFlag = false;

    bool _lastAnimIdle = false;

    // Use this for initialization
    void Start () {
        _player = GameObject.FindGameObjectWithTag(TagName.Player);
    }
	
	// Update is called once per frame
	void Update () {
        // デバック用damage
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Damage();
        }
        AnimatorStateInfo animInfo = _anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log(animInfo.normalizedTime);

        // 弾を撃つの制御
        BulletShotManage();

        DamageCheck();

        // 速い弾を撃つときは各形態の動作をしないようにする
        //if (_speedBulletFlag) { return; }

        // 各形態の動作
        switch (_state)
        {
            case BossState.START:
                StartUpdate();
                break;
            case BossState.LEVEL_1:
                Level_1_Update();
                break;
            case BossState.LEVEL_2:
                Level_2_Update();
                break;
            case BossState.LAST:
                LastUpdate();
                break;
        }
        //Debug.Log(_hp);
        //Debug.Log(_state);
	}

    void NormalShot()
    {
        _attackTime += Time.deltaTime;
        if (_attackTime > _attackInterval)
        {
            // 普通の弾を撃つ
            if (Random.Range(0.0f,1.0f) > _patternRatio)
            {
                // 誘導弾
                var bullet1 = Instantiate(_homingBullet);
                bullet1.transform.position = _sideGunObjPos[0].position;
                bullet1.transform.LookAt(_player.transform);
                bullet1.transform.Rotate(0, -90, 0);
                //Debug.Log("1");

                var bullet2 = Instantiate(_homingBullet);
                bullet2.transform.position = _sideGunObjPos[1].position;         
                bullet2.transform.LookAt(_player.transform);               
                bullet2.transform.Rotate(0, 90, 0);
                //Debug.Log("2");
            }
            else
            {
                // 直線弾
                var bullet = Instantiate(_normalBullet);
                bullet.transform.position = _frontGunObjPos.position;
                bullet.transform.LookAt(_player.transform);
            }
            
            // 数値をリセット
            _attackTime = 0.0f;
        }
    }

    void Damage()
    {
        _hp--;
    }
    void DamageCheck()
    {
        if (_purgeIndex < _parts.Length && _purgeHP[_purgeIndex] == _hp)
        {
            PartsPurge();
            ++_purgeIndex;
        }

        if (_changeStateIndex < _changeStateHP.Length && _changeStateHP[_changeStateIndex] >= _hp)
        {
            if (!_stateChangeFlag)
            {
                _stateChangeFlag = true;
                //_state++;
                ++_changeStateIndex;
            }
        }

        if (_hp == 0)
        {
            Destroy(gameObject);
        }
    }

    void SpeedBulletShot()
    {
        _speedBulletChargeTime += Time.deltaTime;
        if (_speedBulletChargeTime > _speedBulletChargeMax)
        {
            // 速い弾を撃つ
            var bullet = Instantiate(_speedBullet);
            bullet.transform.position = _frontGunObjPos.position;
            bullet.transform.LookAt(_player.transform);

            // 数値たちをリセット
            _speedBulletChargeTime = 0.0f;
            _speedBulletFlag = false;
            _attackTime = 0.0f;

            _anim.speed = 1.0f;
        }
    }

    void BulletShotManage()
    {
        if (_state != BossState.LAST && _state != BossState.START)
        {
            if (_speedBulletIndex < _speedBulletHP.Length && _speedBulletHP[_speedBulletIndex] >= _hp)
            {
                if (!_speedBulletFlag)
                {
                    _speedBulletFlag = true;
                    ++_speedBulletIndex;

                    _anim.speed = 0.0f;
                }
            }

            if (_speedBulletFlag)
            {
                SpeedBulletShot();
            }
            else
            {
                NormalShot();
            }
        }
    }

    void StartUpdate()
    {
        AnimatorStateInfo animInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime >= 1.0f)
        {
            _state++;
            _anim.SetInteger("State", (int)_state);
            //_stateChangeFlag = false;
        }
    }

    void Level_1_Update()
    {
        AnimatorStateInfo animInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime >= 1.0f)
        {
            if (_stateChangeFlag)
            {
                _state++;
                _anim.SetInteger("State", (int)_state);
                _stateChangeFlag = false;
            }
            else
            {
                _anim.Play(Animator.StringToHash("Boss1st"), 0, 0.0f);
            }
        }
    }

    void Level_2_Update()
    {
        AnimatorStateInfo animInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime >= 1.0f)
        {
            if (_stateChangeFlag)
            {
                _state++;
                _anim.SetInteger("State", (int)_state);
                _stateChangeFlag = false;
            }
            else
            {
                _anim.Play(Animator.StringToHash("Boss2nd"), 0, 0.0f);
            }
        }
    }

    void LastUpdate()
    {
        AnimatorStateInfo animInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if (!_lastAnimIdle)
        {         
            if (0.4f < animInfo.normalizedTime && animInfo.normalizedTime < 0.7f)
            {
                SlowMotion._instance.isLimit = false;
                SlowMotion._instance.GameSpeed(0.1f);
            }
            else if (animInfo.normalizedTime >= 0.7f && SlowMotion._instance.isSlow)
            {
                SlowMotion._instance.ResetSpeed();
                _lastAnimIdle = true;
            }           
        }

        if (animInfo.normalizedTime >= 1.0f)
        {
            if (_stateChangeFlag)
            {
                _state++;
                _anim.SetInteger("State", (int)_state);
                _stateChangeFlag = false;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // 出現時は当たらないようにする
        if (_state == BossState.START) { return; }

        // 弾が当たったら体力を減らす
        if (col.gameObject.tag == TagName.Bullet)
        {
            Damage();
        }
    }

    void PartsPurge()
    {
        List<BossPurgeParts> parts = new List<BossPurgeParts>();
        foreach(var part in _parts)
        {
            if (!part.isPurge)
            {
                parts.Add(part);
            }
        }

        if(parts.Count == 0) { return; }

        int rand = Random.Range(0, parts.Count);
        parts[rand].Purge();
    }
}
