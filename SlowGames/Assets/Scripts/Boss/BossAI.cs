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

    // LEVEL_1 の情報 ////////////////////////////////////////////////////////////
    [SerializeField]
    Vector3[] _level_1_pos;
    int _level_1_posIndex = 1;
    int _level_1_posIndexBefore = 1;
    [SerializeField]
    int[] _level_1_moveHp;
    int _level_1_moveHpIndex = 0;
    [SerializeField]
    float _level_1_speed = 5.0f;

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

        // 弾を撃つの制御
        BulletShotManage();

        DamageCheck();

        // 速い弾を撃つときは各形態の動作をしないようにする
        if (_speedBulletFlag) { return; }

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
            _state++;
            ++_changeStateIndex;
        }

        if (_hp == 0)
        {
            GameDirector.instance.isBossDestroy();
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
        if(transform.position.y < 3.5f)
        {
            _state++;
        }
        else
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
    }

    void Level_1_Update()
    {
        if (_level_1_moveHpIndex < _level_1_moveHp.Length && _level_1_moveHp[_level_1_moveHpIndex] >= _hp)
        {
            ++_level_1_moveHpIndex;

            _level_1_posIndexBefore = _level_1_posIndex;
            if (_level_1_posIndex == 0 || _level_1_posIndex == 2)
            {
                _level_1_posIndex = 1;
            }
            else if (_level_1_posIndex == 1)
            {
                var rand = Random.Range(0, 2);
                _level_1_posIndex = (rand == 0) ? 0 : 2;
            }
        }

        var vector = _level_1_pos[_level_1_posIndex] - transform.position;
        if(vector.magnitude > 0.01f)
        {
            Vector3 length = _level_1_pos[_level_1_posIndex] - _level_1_pos[_level_1_posIndexBefore];
            var nomarize = (vector.magnitude / length.magnitude);

            if(_level_1_posIndex == 1 && _level_1_posIndexBefore == 0)
            {
                nomarize *= -1;
            }
            else if(_level_1_posIndex == 2 && _level_1_posIndexBefore == 1)
            {
                nomarize *= -1;
            }

            transform.position += vector.normalized * _level_1_speed * Time.deltaTime
                + Vector3.up * _level_1_speed * Time.deltaTime * Mathf.Sin(nomarize * Mathf.PI)
                 + Vector3.right * _level_1_speed * Time.deltaTime * Mathf.Sin(nomarize * Mathf.PI);
        }
    }

    void Level_2_Update()
    {

    }

    void LastUpdate()
    {

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
