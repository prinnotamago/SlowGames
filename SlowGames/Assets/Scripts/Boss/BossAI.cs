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
    /// ボスの本体
    /// </summary>
    [SerializeField]
    GameObject _bossBodyParent = null;

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
    [SerializeField]
    Transform _SpeedGunObjPos;

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
    [SerializeField]
    float _speedBulletInformTime = 3.0f;

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
        STANDBY,
        START,
        LEVEL_1,
        LEVEL_2,
        LAST,
        CLIMAX,
    }
    [SerializeField]
    BossState _state = BossState.STANDBY;

    // STANDBY の情報 ////////////////////////////////////////////////////////////
    [SerializeField]
    float _standbyTimeMax = 5.0f;
    float _standbyTime = 0.0f;

    // START の情報 //////////////////////////////////////////////////////////////
    [SerializeField]
    Vector3 _startPos;  // 登場するときに向かう場所
    [SerializeField]
    float _startSpeed;  // 落とす速さ
    //////////////////////////////////////////////////////////////////////////////

    // LEVEL_1 の情報 ////////////////////////////////////////////////////////////
    [SerializeField]
    Vector3[] _level_1_pos;          // 各移動場所  
    int _level_1_posIndex = 1;       // 今いる場所のインデックス
    int _level_1_posIndexBefore = 1; // 前いた場所のインデックス
    [SerializeField]
    int[] _level_1_moveHp;        // 各移動場所に動くための HP
    int _level_1_moveHpIndex = 0; // moveHp のインデックス、こいつを変えて移動場所を帰る
    [SerializeField]
    float _level_1_speed = 5.0f;  // 移動の速さ
    float _level_1_moveAngle = 0; // 移動するのに Sin を使ってるのに 角度 で調節している
    bool _isLevel_1_shot = false; // 移動してるときは撃たないようにする
    //////////////////////////////////////////////////////////////////////////////

    // LEVEL_2 の情報/////////////////////////////////////////////////////////////
    [SerializeField]
    float _level_2_maxHeight = 100.0f;    // ハチの字の高さ
    [SerializeField]
    float _level_2_maxWidth = 1.0f;     // ハチの字の横の最大距離
    [SerializeField]
    float _level_2_minWidth = 5.0f;     // ハチの字の横の最小距離
    float _level_2_width = 10.0f;        // 今の八の字での横の長さ
    [SerializeField]
    float _level_2_eightSpeed = 5.0f;        // 移動速度
    float _level_2_moveAngle = 0.0f;       // 移動するのに Sin を使ってるのに 角度 で調節している
    [SerializeField]
    Vector3 _level_2_centerPos;                 // 中心距離
    int _level_2_changeCount = 0;  // ハチの字の横幅を変えるのに使う
    [SerializeField]
    GameObject _level_2_moveObjPrefab;  // ランダムに移動する場所を決めるやつのプレハブ
    List<GameObject> _level_2_moveObjs = new List<GameObject>();  // ランダムに移動する場所を決めるのに使う
    enum Level_2_Mode               // 第２形態のモードを決める
    {
        RANDOM,         // ランダムに動く
        EIGHT_MOVE,     // 8の字に動く
    }
    Level_2_Mode _level_2_mode = Level_2_Mode.RANDOM;
    [SerializeField]
    int[] _level_2_modeChangeHP;    // 第２形態の動きを変えるのに使う
    int _level_2_modeIndex = 0;    // 第２形態の動きを変えるのに使うインデックス
    [SerializeField]
    float _level_2_randomMoveSpeed = 5.0f;  // ランダムの動きの速さ
    [SerializeField]
    float _level_2_randomStopTimeMax = 1.0f;  // ランダムの動きで目的地に着いたらどのくらい止まるか最大値
    float _level_2_randomStopTime = 0.0f;     // ランダムの動きで目的地に着いたらどのくらい止まるか数える
    int _level_2_randomIndex = 0;       // ランダムに移動する場所を入れる
    Vector3 _level_2_movePos;       // ランダム移動で実際に向かう場所
    //////////////////////////////////////////////////////////////////////////////

    // LAST の情報////////////////////////////////////////////////////////////////
    [SerializeField]
    Vector3 _lastTacklePos;       // タックルをする座標
    [SerializeField]
    float _lastTackleSpeed;     // タックルするときの速さ
    List<Vector3> _lastMovePos = new List<Vector3>();   // タックルをする前に移動する場所
    [SerializeField]
    float _lastMoveSpeed;       // 移動するときの速さ
    [SerializeField]
    int _lastMoveNum = 3;         // 移動する回数
    int _lastMoveIndex = 0;       // タックルをする前に移動する場所を操作するためのインデックス
    [SerializeField]
    Vector3 _lastRandMoveLength;  // 移動する場所を決めるのにランダムに使う奥
    bool _lastTackleFlag = false;
    //////////////////////////////////////////////////////////////////////////////

    // CLIMAX の情報//////////////////////////////////////////////////////////////
    Rigidbody _rigidbody;
    [SerializeField]
    float _climaxVelocity = 5.0f;
    [SerializeField]
    float _climaxDestroyTime = 5.0f;
    float _climaxTime = 0.0f;
    [SerializeField]
    int _climaxBoundNum = 2;    // バウンドする回数
    [SerializeField]
    float _climaxBoundPower = 5.0f; // バウンドのパワー
    [SerializeField]
    GameObject _effectPos = null;
    [SerializeField]
    GameObject _boundParticle = null;
    [SerializeField]
    GameObject _slideParticle = null;
    //////////////////////////////////////////////////////////////////////////////

    [SerializeField]
    GameObject _hitParticle = null;

    Animator _anim;

    bool _oneHitFrame = true;

    // Use this for initialization
    void Start () {
        // 狙うためのプレイヤーを探していれる
        _player = GameObject.FindGameObjectWithTag(TagName.Player);

        // ラストで使うタックルをする前に移動する場所を決める
        Vector3 lastRandPos = _lastTacklePos + Vector3.forward * _lastRandMoveLength.z;
        for (int i = 0; i < _lastMoveNum; ++i)
        {
            Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
                                             Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
                                             Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
            Vector3 pos = lastRandPos + randLength;
            _lastMovePos.Add(pos);
        }
        _lastMovePos.Add(_lastTacklePos);

        _rigidbody = /*_bossBody.*/GetComponent<Rigidbody>();

        // 第２形態で使う MoveObj を生成する
        var moveObjParent = Instantiate(_level_2_moveObjPrefab);
        var objs = moveObjParent.GetComponentsInChildren<Transform>();
        for (int i = 0; i < objs.Length; ++i)
        {
            _level_2_moveObjs.Add(objs[i].gameObject);
        }

        // 第２形態の移動場所をあらかじめ決める
        _level_2_randomIndex = Random.Range(0, _level_2_moveObjs.Count);
        var moveObj = _level_2_moveObjs[_level_2_randomIndex];
        float x = moveObj.transform.localScale.x / 2.0f;
        float y = moveObj.transform.localScale.y / 2.0f;
        float z = moveObj.transform.localScale.z / 2.0f;
        _level_2_movePos = new Vector3
            (
                moveObj.transform.position.x + Random.Range(-x, x),
                moveObj.transform.position.y + Random.Range(-y, y),
                moveObj.transform.position.z + Random.Range(-z, z)
            );
        _level_2_randomStopTime = _level_2_randomStopTimeMax;

        // ボス登場ボイス
        AudioManager.instance.stopAllVoice();
        AudioManager.instance.playVoice(AudioName.VoiceName.IV04);

        // ボスと弾が当たるようにする
        Physics.IgnoreLayerCollision(LayerName.Bullet, LayerName.Boss, false);

        // アニメーションを入れる
        _anim = _bossBodyParent.GetComponent<Animator>();
        _anim.Play("PataPata");
    }

    // Update is called once per frame
    void Update () {
        _oneHitFrame = true;

        // デバック用damage
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Damage();
        }

        // STANDBYモードなら時間が過ぎるまで上で待機
        if(_state == BossState.STANDBY)
        {
            _standbyTime += Time.deltaTime;
            if(_standbyTime >= _standbyTimeMax)
            {
                _state = BossState.START;
            }
            return;
        }

        // 弾を撃つの制御
        BulletShotManage();

        // ダメージ判定で起こる処理
        DamageCheck();

        // クライマックス以外プレイヤーに向かせる
        if (_state != BossState.CLIMAX) { _bossBodyParent.transform.LookAt(_player.transform); }

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
            case BossState.CLIMAX:
                ClimaxUpdate();
                break;
        }
        //Debug.Log(_hp);
        //Debug.Log(_state);
	}

    // 一定時間ごとに普通の弾を撃つ
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

    // ダメージを受ける
    void Damage()
    {
        if (!_oneHitFrame) { return; }
        _hp--;
        _oneHitFrame = false;
    }
    // ダメージが一定受けたらの処理たち
    void DamageCheck()
    {
        // 一定 HP きったらパーツをパージさせる
        if (_purgeIndex < _parts.Length && _purgeHP[_purgeIndex] == _hp)
        {
            PartsPurge();
            ++_purgeIndex;
        }

        // 第２形態ならモードチェンジの判定
        if(_state == BossState.LEVEL_2)
        {
            // 一定ダメージを受けたらハチの字モードに変える
            if (_level_2_modeIndex < _level_2_modeChangeHP.Length && _level_2_modeChangeHP[_level_2_modeIndex] >= _hp)
            {
                if (0 == _level_2_modeIndex)
                {
                    // クライマックスになったボイスを流す
                    AudioManager.instance.stopAllVoice();
                    AudioManager.instance.playVoice(AudioName.VoiceName.IV10);
                }

                _level_2_mode = Level_2_Mode.EIGHT_MOVE;
                ++_level_2_modeIndex;
            }
        }

        // 一定 HP きったら形態を変える
        if (_changeStateIndex < _changeStateHP.Length && _changeStateHP[_changeStateIndex] >= _hp)
        {
            _state++;
            ++_changeStateIndex;

            if(_state == BossState.LEVEL_2)
            {
                // 第２形態になったボイスを流す
                AudioManager.instance.stopAllVoice();
                AudioManager.instance.playVoice(AudioName.VoiceName.IV09);
            }
            else if(_state == BossState.LAST)
            {
                // 突っ込んでくることを伝えるボイス
                AudioManager.instance.stopAllVoice();
                AudioManager.instance.playVoice(AudioName.VoiceName.IV11);
            }
            else if(_state == BossState.CLIMAX)
            {
                // ボスを倒したことを伝えるボイス
                AudioManager.instance.stopAllVoice();
                AudioManager.instance.playVoice(AudioName.VoiceName.IV14);

                //_anim.SetBool("down", true);
                _anim.Play("Down");
            }
        }

        //// HP が 0 になったら死ぬ
        //if (_hp == 0)
        //{
        //    Destroy(gameObject);
        //    GameDirector.instance.isBossDestroy();
        //}
    }

    // 速い球を撃つ
    void SpeedBulletShot()
    {
        if(_speedBulletChargeTime == 0.0f)
        {
            // 速い弾を撃つ
            var bullet = Instantiate(_speedBullet);
            bullet.transform.position = _frontGunObjPos.position;
            bullet.transform.LookAt(_player.transform);
            bullet.GetComponent<BossBullet>().chargeTime = _speedBulletChargeMax;
        }

        _speedBulletChargeTime += Time.unscaledDeltaTime;

        if (_speedBulletChargeTime > _speedBulletChargeMax)
        {
            // 数値たちをリセット
            _speedBulletChargeTime = 0.0f;
            _speedBulletFlag = false;
            _attackTime = 0.0f;


            // 第２形態ならランダム移動に戻す
            if (_level_2_mode == Level_2_Mode.EIGHT_MOVE)
            {
                _level_2_mode = Level_2_Mode.RANDOM;
            }
        }
    }

    // 撃つ弾を管理する
    void BulletShotManage()
    {
        // 登場とクライマックスでも弾を撃たない
        if (_speedBulletFlag || (_state != BossState.LAST && _state != BossState.START && _state != BossState.CLIMAX))
        {
            // LEVEL_1 で移動中は弾を撃たないようにするので抜ける
            if (_state == BossState.LEVEL_1 && !_isLevel_1_shot) { return; }

            if (_speedBulletIndex < _speedBulletHP.Length && _speedBulletHP[_speedBulletIndex] >= _hp)
            {
                if (!_speedBulletFlag)
                {
                    _speedBulletFlag = true;
                    ++_speedBulletIndex;

                    // 高速弾を準備してることを伝える
                    AudioManager.instance.stopAllVoice();
                    AudioManager.instance.playVoice(AudioName.VoiceName.IV06);
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

    // 登場形態
    void StartUpdate()
    {
        //if(transform.position.y < 3.5f)
        //{
        //    _state++;
        //}
        //else
        //{
        //    transform.position += Vector3.down * Time.deltaTime;
        //}

        var vector = _startPos - _bossBodyParent.transform.position;
        _bossBodyParent.transform.position += vector / _startSpeed;
        if(vector.magnitude <= 0.5f)
        {
            _state++;
        }
    }

    // 第一形態
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

            if (_level_1_posIndexBefore == 1 && _level_1_posIndex == 0)
            {
                _level_1_moveAngle = 0;
            }
            else if (_level_1_posIndexBefore == 2 && _level_1_posIndex == 1)
            {
                _level_1_moveAngle = Mathf.PI;
            }
            else if (_level_1_posIndexBefore == 0 && _level_1_posIndex == 1)
            {
                _level_1_moveAngle = Mathf.PI / 4;
            }
            else if (_level_1_posIndexBefore == 1 && _level_1_posIndex == 2)
            {
                _level_1_moveAngle = (-Mathf.PI / 4) * 2;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    _level_1_posIndexBefore = 1;
        //    _level_1_posIndex = 0;
        //    _level_1_moveAngle = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    _level_1_posIndexBefore = 2;
        //    _level_1_posIndex = 1;
        //    _level_1_moveAngle = Mathf.PI;
        //}


        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    _level_1_posIndexBefore = 0;
        //    _level_1_posIndex = 1;
        //    _level_1_moveAngle = Mathf.PI / 4;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    _level_1_posIndexBefore = 1;
        //    _level_1_posIndex = 2;
        //    _level_1_moveAngle = (-Mathf.PI / 4) * 2;
        //}

        var vector = _level_1_pos[_level_1_posIndex] - _bossBodyParent.transform.position;

        Vector3 length = _level_1_pos[_level_1_posIndex] - _level_1_pos[_level_1_posIndexBefore];
        var nomarize = (vector.magnitude / length.magnitude);

        // 0.4f は微調整の数値
        _level_1_moveAngle += _level_1_speed * Time.deltaTime * 0.4f;

        _bossBodyParent.transform.position += new Vector3(
               vector.normalized.x * _level_1_speed * Time.deltaTime + Mathf.Cos(_level_1_moveAngle) * _level_1_speed * Time.deltaTime,
               vector.normalized.y * _level_1_speed * Time.deltaTime + Mathf.Sin(_level_1_moveAngle) * _level_1_speed * Time.deltaTime,
               vector.normalized.z * _level_1_speed * Time.deltaTime
               );

        //Debug.Log(vector.magnitude);
        if (vector.magnitude < 0.5f)
        {
            _isLevel_1_shot = true;
        }
        else
        {
            _isLevel_1_shot = false;
        }
    }

    // 第二形態
    void Level_2_Update()
    {
        // ハチの字の動き
        if (_level_2_mode == Level_2_Mode.EIGHT_MOVE)
        {
            _level_2_moveAngle += Time.deltaTime * _level_2_eightSpeed;

            if (_level_2_changeCount == 0 && _level_2_moveAngle > (Mathf.PI / 2))
            {
                _level_2_changeCount++;
                _level_2_width = Random.Range(_level_2_minWidth, _level_2_maxWidth);
            }
            else if (_level_2_changeCount == 1 && _level_2_moveAngle > (Mathf.PI / 2) * 3)
            {
                _level_2_changeCount++;
                _level_2_width = Random.Range(_level_2_minWidth, _level_2_maxWidth);
            }
            else if (_level_2_changeCount == 2 && _level_2_moveAngle > Mathf.PI * 2)
            {
                _level_2_moveAngle = 0.0f;
                _level_2_changeCount = 0;
            }
            //Debug.Log(_level_2_moveAngle);

            var nextPos = _level_2_centerPos + new Vector3(
                Mathf.Cos(_level_2_moveAngle) * _level_2_width,
                Mathf.Sin(_level_2_moveAngle * 2) * _level_2_maxHeight,
                0);

            var vector = nextPos - _bossBodyParent.transform.position;

            _bossBodyParent.transform.position += vector * 3.0f * Time.deltaTime;
        }
        // ランダムに動く動き
        else if(_level_2_mode == Level_2_Mode.RANDOM)
        {
            var vector = _level_2_movePos - _bossBodyParent.transform.position;

            // 向かう場所に近づいたら
            if (vector.magnitude < 0.1f)
            {
                // 止まる時間を減らす
                _level_2_randomStopTime -= Time.deltaTime;

                // 止まる時間を過ぎたら向かう場所を再設定
                if (_level_2_randomStopTime <= 0.0f)
                {
                    while (true)
                    {
                        int beforeIndex = _level_2_randomIndex;
                        _level_2_randomIndex = Random.Range(0, _level_2_moveObjs.Count);
                        if(beforeIndex != _level_2_randomIndex) { break; }
                    }
                    
                    var moveObj = _level_2_moveObjs[_level_2_randomIndex];
                    float x = moveObj.transform.localScale.x / 2.0f;
                    float y = moveObj.transform.localScale.y / 2.0f;
                    float z = moveObj.transform.localScale.z / 2.0f;
                    _level_2_movePos = new Vector3
                        (
                            moveObj.transform.position.x + Random.Range(-x, x),
                            moveObj.transform.position.y + Random.Range(-y, y),
                            moveObj.transform.position.z + Random.Range(-z, z)
                        );
                    _level_2_randomStopTime = _level_2_randomStopTimeMax;
                }
            }
            // 向かう場所についてなかったらそこに移動する
            else
            {
                // こっちの動きだせぇ
                //Debug.Log(_level_2_randomIndex);
                //transform.position += vector.normalized * _level_2_randomMoveSpeed * Time.deltaTime;

                _bossBodyParent.transform.position += vector * _level_2_randomMoveSpeed * Time.deltaTime;
            }
        }

    }

    // ラスト(突撃)
    void LastUpdate()
    {
        //var test = (_level_1_pos[1] + Vector3.one * Random.Range(-1, 1)) - transform.position;
        //transform.position += test / 10.0f;

        // 移動する場所に動く処理
        if(_lastMoveIndex < _lastMovePos.Count)
        {
            var length = _lastMovePos[_lastMoveIndex] - _bossBodyParent.transform.position;
            _bossBodyParent.transform.position += length * _lastMoveSpeed * Time.deltaTime;
            if (length.magnitude < 1.5f)
            {
                // タックルをしようとしていたらボイスを流す
                //if ((10) == _lastMoveIndex)
                //{
                //    AudioManager.instance.stopAllVoice();
                //    AudioManager.instance.playVoice(AudioName.VoiceName.IV12);
                //}

                ++_lastMoveIndex;

                if (_lastMoveIndex == _lastMovePos.Count - 2)
                {
                    //_anim.SetBool("tackle", true);
                    _anim.Play("Tackle");
                }
            }
        }
        // プレイヤーへのタックル
        else
        {
            // スローじゃなかったらスローにする
            if (!SlowMotion._instance.isSlow)
            {
                // タックルフラグをオンに
                _lastTackleFlag = true;

                SlowMotion._instance.GameSpeed(0.1f);
                SlowMotion._instance.isLimit = false;

                // 撃ち続けてボイスを流す
                if (!SlowMotion._instance.isLimit)
                {
                    AudioManager.instance.stopAllVoice();
                    AudioManager.instance.playVoice(AudioName.VoiceName.IV13);
                }

                
            }
            var length = _player.transform.position - _bossBodyParent.transform.position;
            _bossBodyParent.transform.position += length * _lastTackleSpeed * Time.deltaTime;
        }
    }

    // クライマックス
    void ClimaxUpdate()
    {
        // スローだったら解除
        if (SlowMotion._instance.isSlow)
        {
            SlowMotion._instance.ResetSpeed();
            SlowMotion._instance.isLimit = true;

            // タックルフラグを切る
            _lastTackleFlag = false;

            // 弾がボスに当たらないようにする
            Physics.IgnoreLayerCollision(LayerName.Bullet, LayerName.Boss, true);
        }

        if (!_rigidbody.useGravity)
        {
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            var vector = _bossBodyParent.transform.position - _player.transform.position;
            _rigidbody.velocity = vector.normalized * _climaxVelocity;
        }

        _climaxTime += Time.deltaTime;
        if(_climaxDestroyTime < _climaxTime)
        {
            Damage();

            // HP が 0 になったら死ぬ
            if (_hp <= 0)
            {
                Destroy(gameObject);
                GameDirector.instance.isBossDestroy();
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // クライマックスなら
        if (_state == BossState.CLIMAX)
        {
            if (col.gameObject.tag == TagName.Floor)
            {
                if (_climaxBoundNum > 0)
                {
                    // バウンドの回数を減らす
                    _climaxBoundNum--;
                    
                    // バウンドの力を半分に
                    _climaxBoundPower *= 0.5f;

                    // ２次元での向きを出す
                    var bossPos = new Vector2(_bossBodyParent.transform.position.x, _bossBodyParent.transform.position.z);
                    var playerPos = new Vector2(_player.transform.position.x, _player.transform.position.z);
                    var vector2 = bossPos - playerPos;

                    // ３次元に変換
                    var vector = new Vector3(vector2.x, 0.0f, vector2.y);

                    // その方向に飛ばす
                    _rigidbody.velocity = vector.normalized * _climaxBoundPower;
                    _rigidbody.velocity += Vector3.up * _climaxBoundPower;

                    // 最後のバウンドの時滑らせるために強い力をあたえる
                    if (_climaxBoundNum == 0)
                    {
                        _rigidbody.velocity += vector.normalized * _climaxVelocity;
                    }

                    // バウンドエフェクトを出す
                    var boundParticle = Instantiate(_boundParticle);
                    boundParticle.transform.position = _effectPos.transform.position;
                }
                else
                {
                    // 滑るを出す
                    var boundParticle = Instantiate(_slideParticle);
                    boundParticle.transform.position = _effectPos.transform.position;
                    boundParticle.transform.LookAt(_player.transform);
                    boundParticle.transform.parent = transform;
                }
            }
            return;
        }
    }

    void EffectAndDamage(Collider col)
    {
        var particle = Instantiate(_hitParticle);
        particle.transform.position = col.transform.position;

        // 出現時は当たらないようにする
        if (_state == BossState.START || _state == BossState.CLIMAX || _state == BossState.STANDBY) { return; }

        // 高速弾を撃つときはエフェクトだけを出す
        if ((_state != BossState.LAST && !_speedBulletFlag) || (_state == BossState.LAST && _lastTackleFlag))
        {
            Damage();
        }
    }

    public void PargeDamage(Collider col)
    {
        if (col.gameObject.tag == TagName.Bullet)
        {
            EffectAndDamage(col);
        }
    }

    void OnTriggerEnter(Collider col)
    {

        // 弾が当たったら体力を減らす
        if (col.gameObject.tag == TagName.Bullet)
        {
            EffectAndDamage(col);
        }
        // プレイヤーと当たったら
        else if (col.gameObject.tag == TagName.Player)
        {
            // 即死させる
            _player.GetComponent<PlayerHP>().Damage(1000000000);
        }
    }

    // パーツをランダムでパージする
    void PartsPurge()
    {
        //List<BossPurgeParts> parts = new List<BossPurgeParts>();
        //foreach(var part in _parts)
        //{
        //    if (!part.isPurge)
        //    {
        //        parts.Add(part);
        //    }
        //}

        //if(parts.Count == 0) { return; }

        //int rand = Random.Range(0, parts.Count);
        //parts[rand].Purge();

        _parts[_purgeIndex].Purge();
    }
}
