using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    /// <summary>
    /// ボスの体力
    /// </summary>
    [SerializeField]
    int _hp = 10;
    int _hpMax = 0;

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
    /// ボスのエミッションをいじる
    /// </summary>
    [SerializeField]
    Material[] _mat;
    float _changeColorSpeed = 1.0f;
    float _changeColorTime = 0.0f;

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

    bool _normalBulletFlag = true;  // 通常弾を撃つフラグ

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
    [SerializeField]
    GameObject _standbySandParticle = null;

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
    float _level_2_Z = 5.0f;     // ハチの字の横の最小距離
    [SerializeField]
    float _level_2_eightSpeed = 5.0f;        // 移動速度
    float _level_2_moveAngle = 0.0f;       // 移動するのに Sin を使ってるのに 角度 で調節している
    float _level_2_moveAngle_Z = 0.0f;
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
    Vector3 _lastTackleBeforePos;       // タックルをする座標
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
    [SerializeField]
    float _lastRandomStopTimeMax = 1.0f;  // ランダムの動きで目的地に着いたらどのくらい止まるか最大値
    float _lastRandomStopTime = 0.0f;     // ランダムの動きで目的地に着いたらどのくらい止まるか数える
    Vector3 _lastRandomVectorPos;
    bool _lastTackleFlag = false;

    //Bezier _bezier;
    //float _bezierTime = 0.0f;
    //float _bezierTimeSpeed = 1.0f;

    //// 自由に飛び回る
    //[SerializeField]
    //float _lastFlyTime = 10.0f;
    //[SerializeField]
    //Vector3 _lastRotateSenterPos;
    //float _lastRotateAngle = 0.0f;
    //[SerializeField]
    //float _lastRotateSpeed = 5.0f;
    //[SerializeField]
    //float _lastRotateLength = 10.0f;
    //[SerializeField]
    //float _lastRotateLengthDownSpeed = 1.0f;
    //[SerializeField]
    //float _lastRotateUpSpeed = 1.0f;

    //// タックル前の予備動作
    ////[SerializeField]
    ////Vector3[] _lastBeforeTacklePos;
    ////int _lastBeforeTacklePosIndex = 0;
    ////[SerializeField]
    ////float _lastBeforeTackleSpeed = 5.0f;
    //[SerializeField]
    //float _lastBeforeSleepTime = 3.0f;
    //[SerializeField]
    //float _lastBrforeSleepDownSpeed = 1.0f;
    ////bool _lastSleepVoiceFlag = true;

    //[SerializeField]
    //Vector3 _lastBeforeTackleSenterPos;
    //float _lastBeforeRotateAngle = 0.0f;
    //const float _LAST_BEFORE_ROTATE_ANGLE_MAX = -Mathf.PI * 3;
    //[SerializeField]
    //float _lastBeforeRotateSpeed;
    //[SerializeField]
    //float _lastBeforeMoveSpeed;
    //[SerializeField]
    //float _lastBeforeLengthY;
    //[SerializeField]
    //float _lastBeforeLengthZ;

    [SerializeField]
    Vector3 _lastPreparationPos;
    Vector3 _lastPreparationRandPos;
    [SerializeField]
    float _lastPreparationAngleSpeed = 1.0f;
    [SerializeField]
    float _lastPreparationAngleAcceleration = 1.0f;
    float _lastPreparationAngle = 0.0f;
    [SerializeField]
    float _lastPreparationMoveSpeed = 1.0f;
    [SerializeField]
    float _lastPreparationTime = 10.0f;
    [SerializeField]
    float _lastPreparationPoseTime = 5.0f;

    [SerializeField]
    float _lastStaggerSpeed = 1.0f;
    //[SerializeField]
    //float _lastStaggerAngleSpeed = 2.0f;
    float _lastStaggerAngle = 1.0f;
    [SerializeField]
    float _lastStaggerLength = 1.0f;

    bool _lastPreparationFlag = false;

    //[SerializeField]
    float _lastParticleCreateTime = 1000.0f;
    [SerializeField]
    float _lastParticleInterval = 1;
    [SerializeField]
    Vector3 _lastParticleRandPos;

    //[SerializeField]
    float _lastDownRotate = 0.0f;
    [SerializeField]
    float _lastDownRotateMax = 0.0f;
    [SerializeField]
    float _lastDownSpeed = 1.0f;

    [SerializeField]
    float _lastSleepTime = 3.0f;

    [SerializeField]
    float _lastSlowStartTime = 1.0f;
    //////////////////////////////////////////////////////////////////////////////

    // CLIMAX の情報//////////////////////////////////////////////////////////////
    Rigidbody _rigidbody;
    [SerializeField]
    float _climaxVelocity = 5.0f;
    [SerializeField]
    float _climaxDestroyTime = 5.0f;
    float _climaxTime = 0.0f;
    //[SerializeField]
    //int _climaxBoundNum = 2;    // バウンドする回数
    //[SerializeField]
    //float _climaxBoundPowerSide = 5.0f; // バウンドのパワー
    //[SerializeField]
    //float _climaxBoundPowerUp = 5.0f; // バウンドのパワー
    //[SerializeField]
    //GameObject _effectPos = null;
    //[SerializeField]
    //GameObject _boundParticle = null;
    //[SerializeField]
    //GameObject _slideParticle = null;
    //[SerializeField]
    //GameObject _slideParticleParent = null;
    [SerializeField]
    float _climaxUpPower = 10.0f;
    [SerializeField]
    float _climaxFirstDebrisNum = 1.0f;
    [SerializeField]
    float _climaxFirstDebrisSize = 1.0f;
    [SerializeField]
    float _climaxLastDebrisNum = 1.0f;
    [SerializeField]
    float _climaxLastDebrisSize = 1.0f;
    [SerializeField]
    GameObject _climaxDebrisPrefab = null;
    [SerializeField]
    GameObject _climaxExplosion = null;
    //////////////////////////////////////////////////////////////////////////////

    // ヒットエフェクト
    [SerializeField]
    GameObject _hitParticle = null;

    // ラストでタックルされてるときに出すヒットエフェクト
    [SerializeField]
    GameObject _hitLastParticle = null;

    Animator _anim;

    bool _oneHitFrame = true;   // 1フレームに複数回ダメージを受けるのを防ぐ

    [SerializeField]
    bool _debugFlag = false;

    void Awake()
    {
        if (_debugFlag)
        {
            VoiceNumberStorage.setVoice();
        }
    }

    // Use this for initialization
    void Start () {
        // HP の最大値保存
        _hpMax = _hp;

        // 狙うためのプレイヤーを探していれる
        _player = GameObject.FindGameObjectWithTag(TagName.Player);

        // ラストで使うタックルをする前に移動する場所を決める
        //Vector3 lastRandPos = _lastTacklePos + Vector3.forward * _lastRandMoveLength.z;
        //for (int r = 0; r < 3; ++r)
        //{
        //    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                     Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                     Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //    Vector3 pos = lastRandPos + randLength;
        //    _lastMovePos.Add(pos);
        //}
        //for (int i = 0; i < _lastMoveNum; ++i)
        //{

        //    if (_lastMoveNum == i + 1)
        //    {
        //        for (int r = 0; r < 2; ++r)
        //        {

        //            if (i % 2 == 0)
        //            {
        //                if (r % 2 == 0)
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                         Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                         Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //                else
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                            Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                            Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //            }
        //            else
        //            {
        //                if (r % 2 == 1)
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                       Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                       Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //                else
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                        Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                        Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //            }
        //        }
        //        _lastMovePos.Add(_lastTackleBeforePos);
        //    }
        //    else
        //    {
        //        for (int r = 0; r < 2; ++r)
        //        {
        //            if (i % 2 == 0)
        //            {
        //                if (r % 2 == 0)
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                          Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                          Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //                else
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                       Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                       Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //            }
        //            else
        //            {
        //                if (r % 2 == 1)
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                           Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                           Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //                else
        //                {
        //                    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                                         Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                                         Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //                    Vector3 pos = lastRandPos + randLength;
        //                    _lastMovePos.Add(pos);
        //                }
        //            }
        //        }
        //        {
        //            Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x / 10, _lastRandMoveLength.x / 10),
        //                                                 Random.Range(-_lastRandMoveLength.y / 10, _lastRandMoveLength.y / 10),
        //                                                 Random.Range(-_lastRandMoveLength.z / 10, _lastRandMoveLength.z / 10));
        //            Vector3 pos = lastRandPos + randLength;
        //            _lastMovePos.Add(pos);
        //        }
        //    }

        //    //{
        //    //    var vector = _lastMovePos[i - 1] - _lastMovePos[i - 2];
        //    //    _lastMovePos.Add(_lastMovePos[i - 1] + vector.normalized);
        //    //}
        //    //{
        //    //    var vector = _lastMovePos[i] - _lastMovePos[i - 1];
        //    //    _lastMovePos.Add(_lastMovePos[i] + vector.normalized);
        //    //}

        //    //for (int r = 0; r < 2; ++r)
        //    //{
        //    //    if (i == _lastMoveNum * 4 - 4 - 1)
        //    //    {
        //    //        if (r == 1)
        //    //        {
        //    //            _lastMovePos.Add(_lastTackleBeforePos);
        //    //        }
        //    //        else
        //    //        {
        //    //            Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //    //                                    Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //    //                                    Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //    //            Vector3 pos = lastRandPos + randLength;
        //    //            _lastMovePos.Add(pos);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //    //                                     Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //    //                                     Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //    //        Vector3 pos = lastRandPos + randLength;
        //    //        _lastMovePos.Add(pos);
        //    //    }
        //    //}

        //    //if(i == _lastMoveNum - 1)
        //    //{
        //    //    //var vector = _lastMovePos[i + 2] - _lastMovePos[i + 1];
        //    //}
        //    //else
        //    //{
        //    //    _lastMovePos.Add(_lastTackleBeforePos);
        //    //}
        //}
        //_lastMovePos.Add(_lastTacklePos);

        //for (int i = 0; i < _lastMoveNum; ++i)
        //{

        //    Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
        //                                 Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
        //                                 Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        //    Vector3 pos = lastRandPos + randLength;
        //    _lastMovePos.Add(pos);
        //}
        //_lastMovePos.Add(_lastPreparationPos);

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
        _lastRandomStopTime = _lastRandomStopTimeMax;
        _lastRandomVectorPos = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));

        // ボス登場ボイス
        AudioManager.instance.stopAllVoice();
        AudioManager.instance.playVoice(AudioName.VoiceName.IV04);

        // ボスと弾が当たるようにする
        Physics.IgnoreLayerCollision(LayerName.Bullet, LayerName.Boss, false);

        // アニメーションを入れる
        _anim = _bossBodyParent.GetComponent<Animator>();
        _anim.Play("PataPata");

        // サンドを落とす
        Instantiate(_standbySandParticle);

        Vector3 lastRandPos = _lastTacklePos + Vector3.forward * _lastRandMoveLength.z;
        Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
                                                                 Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
                                                                 Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
        _lastPreparationRandPos = lastRandPos + randLength;
    }

    // Update is called once per frame
    void Update () {
        _changeColorTime += Time.deltaTime * _changeColorSpeed;
        float sin = Mathf.Abs(Mathf.Sin(_changeColorTime));
        EmissionColorChange(Color.red, Color.black, sin);

        // そのフレーム中１回しか当たらないようにする
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

                // ボス登場音
                AudioManager.instance.playSe(AudioName.SeName.BossApparance);
            }
            return;
        }

        // 弾を撃つの制御
        BulletShotManage();

        // ダメージ判定で起こる処理
        DamageCheck();

        // クライマックス以外プレイヤーに向かせる
        //if (_state != BossState.CLIMAX) { _bossBodyParent.transform.LookAt(_player.transform); }

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

        if (_hpMax / 2 == _hp)
        {
            // HP50%切ったら１度だけ優性ボイス
            AudioManager.instance.stopAllVoice();
            AudioManager.instance.playVoice(AudioName.VoiceName.IV10);
        }
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
                //if (0 == _level_2_modeIndex)
                //{
                //    // HP50%切ったら１度だけ優性ボイス
                //    AudioManager.instance.stopAllVoice();
                //    AudioManager.instance.playVoice(AudioName.VoiceName.IV10);
                //}

                _level_2_mode = Level_2_Mode.EIGHT_MOVE;
                //_anim.Play("Tackle");
                ++_level_2_modeIndex;

                // 移動音
                AudioManager.instance.play3DSe(gameObject, AudioName.SeName.BossMove, true);
            }
        }

        // 一定 HP きったら形態を変える
        if (_changeStateIndex < _changeStateHP.Length && _changeStateHP[_changeStateIndex] >= _hp)
        {
            _state++;
            ++_changeStateIndex;

            // 移動音を止める
            AudioManager.instance.stop3DSe(gameObject, AudioName.SeName.BossMove);

            if (_state == BossState.LEVEL_2)
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
                //_anim.Play("Down");
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
            bullet.transform.parent = transform;
            bullet.transform.position = _SpeedGunObjPos.position;
            bullet.transform.LookAt(_player.transform);
            bullet.GetComponent<BossBullet>().chargeTime = _speedBulletChargeMax;
        }

        _speedBulletChargeTime += Time.unscaledDeltaTime;

        // エミッションを変える速度を上げる
        _changeColorTime += Time.deltaTime * _changeColorSpeed * 10;
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
                //_anim.Play("Up");

                // 移動音を止める
                AudioManager.instance.stop3DSe(gameObject, AudioName.SeName.BossMove);
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
                }
            }

            if (_speedBulletFlag)
            {
                SpeedBulletShot();
            }

            if (_normalBulletFlag)
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

        _bossBodyParent.transform.LookAt(_player.transform);

        var vector = _startPos - _bossBodyParent.transform.position;
        _bossBodyParent.transform.position += vector / _startSpeed;
        if(vector.magnitude <= 0.5f)
        {
            _state++;
        }
    }
    [SerializeField]
    float anglex = Mathf.PI / 4;
    [SerializeField]
    float anglex2 = Mathf.PI / 4;
    // 第一形態
    void Level_1_Update()
    {
        //_bossBodyParent.transform.LookAt(_player.transform);
        _bossBodyParent.transform.LookAt(_player.transform);

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

            // 移動音
            AudioManager.instance.play3DSe(gameObject, AudioName.SeName.BossMove);
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

            // 移動音を止める
            AudioManager.instance.stop3DSe(gameObject, AudioName.SeName.BossMove);
        }
        else
        {
            _isLevel_1_shot = false;

            // 移動音
            //if (!AudioManager.instance.findSeSource(AudioName.SeName.BossMove).isPlaying)
            //{
            //    AudioManager.instance.play3DSe(gameObject, AudioName.SeName.BossMove);
            //}
        }
    }

    // 第二形態
    void Level_2_Update()
    {
        _bossBodyParent.transform.LookAt(_player.transform);
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

            if (_level_2_moveAngle <= (Mathf.PI / 2) || _level_2_moveAngle > (Mathf.PI / 2) * 3)
            {
                _level_2_moveAngle_Z += Time.deltaTime * _level_2_eightSpeed;
            }
            else if(_level_2_moveAngle > (Mathf.PI / 2) || (_level_2_moveAngle >= (Mathf.PI / 2) && _level_2_moveAngle <= (Mathf.PI / 2) * 3))
            {
                _level_2_moveAngle_Z -= Time.deltaTime * _level_2_eightSpeed;
            }
            

            var nextPos = _level_2_centerPos + new Vector3(
                Mathf.Cos(_level_2_moveAngle) * _level_2_width,
                Mathf.Sin(_level_2_moveAngle * 2) * _level_2_maxHeight,
                //0,
                //Mathf.Abs(Mathf.Cos(_level_2_moveAngle)) / 1.0f * -10.0f - Mathf.Sin(_level_2_moveAngle) * 3,
                Mathf.Cos(_level_2_moveAngle_Z) * -_level_2_Z
                );

            var vector = nextPos - _bossBodyParent.transform.position;

            _bossBodyParent.transform.position += vector * 3.0f * Time.deltaTime;

            //_bossBodyParent.transform.LookAt(nextPos);
            //_bossBodyParent.transform.LookAt(_player.transform);

            //_bossBodyParent.transform.Rotate(0, 0,-_level_2_moveAngle * Mathf.Rad2Deg);



            //Quaternion q = Quaternion.LookRotation(nextPos);
            //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 36000.0f * Time.deltaTime);

            //if (_level_2_moveAngle <= (Mathf.PI / 2) || _level_2_moveAngle > (Mathf.PI / 2) * 3)
            //{
            //    _bossBodyParent.transform.Rotate(0, 0, -Time.deltaTime * _level_2_eightSpeed * Mathf.Rad2Deg * 1.5f);
            //}
            //else if (_level_2_moveAngle > (Mathf.PI / 2) || (_level_2_moveAngle >= (Mathf.PI / 2) && _level_2_moveAngle <= (Mathf.PI / 2) * 3))
            //{
            //    _bossBodyParent.transform.Rotate(0, 0, Time.deltaTime * _level_2_eightSpeed * Mathf.Rad2Deg * 1.5f);
            //}

            //Debug.Log(_level_2_moveAngle);
            //if (_level_2_moveAngle <= Mathf.PI)
            //{
            //    _bossBodyParent.transform.Rotate(0, 0, _level_2_moveAngle * Mathf.Rad2Deg);
            //}
            //else
            //{
            //    _bossBodyParent.transform.Rotate(0, 0, _level_2_moveAngle * Mathf.Rad2Deg);
            //}
        }
        // ランダムに動く動き
        else if(_level_2_mode == Level_2_Mode.RANDOM)
        {
            //_bossBodyParent.transform.LookAt(_player.transform);

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

                // 移動音
                AudioManager.instance.stop3DSe(gameObject, AudioName.SeName.BossMove);
                AudioManager.instance.play3DSe(gameObject, AudioName.SeName.BossMove);
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
        //if(_lastMoveIndex < _lastMovePos.Count)
        //{
        //    var length = _lastMovePos[_lastMoveIndex] - _bossBodyParent.transform.position;
        //    _bossBodyParent.transform.position += length * _lastMoveSpeed * Time.deltaTime;
        //    if (length.magnitude < 1.5f)
        //    {
        //        // タックルをしようとしていたらボイスを流す
        //        //if ((10) == _lastMoveIndex)
        //        //{
        //        //    AudioManager.instance.stopAllVoice();
        //        //    AudioManager.instance.playVoice(AudioName.VoiceName.IV12);
        //        //}

        //        //Quaternion q = Quaternion.LookRotation(_bossBodyParent.transform.position + _lastRandomVectorPos);
        //        //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 36000.0f * Time.deltaTime);
        //        //_bossBodyParent.transform.Rotate(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

        //        _lastRandomStopTime -= Time.deltaTime;

        //        if (_lastRandomStopTime <= 0.0f)
        //        {

        //            _lastRandomStopTime = _lastRandomStopTimeMax;
        //            //_lastRandomVectorPos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));

        //            ++_lastMoveIndex;

        //            if (_lastMoveIndex == _lastMovePos.Count)
        //            {
        //                //_anim.SetBool("tackle", true);
        //                _anim.Play("Tackle");
        //            }
        //            else if (_lastMoveIndex == _lastMovePos.Count)
        //            {
        //                // 撃ち続けてボイスを流す
        //                AudioManager.instance.stopAllVoice();
        //                AudioManager.instance.playVoice(AudioName.VoiceName.IV13);
        //            }
        //        }
        //    }
        //}

        //const float randMinMax = 10.0f;
        //_bossBodyParent.transform.Translate(
        //    new Vector3(
        //          Random.Range(-randMinMax, randMinMax)
        //        , Random.Range(-randMinMax, randMinMax)
        //        , Random.Range(-randMinMax, randMinMax)
        //        ) * Time.deltaTime
        //        );

        //// 自由に飛び回る
        //if (_lastFlyTime > 0)
        //{
        //    if(_lastRotateAngle == 0)
        //    {
        //        _anim.SetBool("tackle", true);
        //        _anim.Play("Tackle");
        //    }

        //    _lastFlyTime -= Time.deltaTime;

        //    _lastRotateAngle += _lastRotateSpeed * Time.deltaTime;

        //    var offset = new Vector3(Mathf.Sin(_lastRotateAngle), 0, Mathf.Cos(_lastRotateAngle)) * _lastRotateLength;
        //    var nextPos = _lastRotateSenterPos + offset;

        //    var length = nextPos - _bossBodyParent.transform.position;
        //    _bossBodyParent.transform.position += length * _lastRotateSpeed * Time.deltaTime;

        //    _lastRotateLength -= _lastRotateLengthDownSpeed * Time.deltaTime;
        //    _lastRotateSenterPos += Vector3.up * _lastRotateUpSpeed * Time.deltaTime;

        //    Quaternion q = Quaternion.LookRotation(length);
        //    _bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 3600.0f * Time.deltaTime);

        //    if (_lastFlyTime <= 0)
        //    {
        //        _bezier = new Bezier(_bossBodyParent.transform.position, _lastMovePos[_lastMoveIndex], _lastMovePos[_lastMoveIndex + 1], _lastMovePos[_lastMoveIndex + 2]);
        //        //_bezier = new Bezier(_lastMovePos[0], _lastMovePos[1], _lastMovePos[2], _lastMovePos[3]);
        //    }
        //}
        //else if (_lastMoveIndex < _lastMovePos.Count)
        //{
        //    //_bossBodyParent.transform.LookAt(_player.transform);

        //    //var length = _lastMovePos[_lastMoveIndex] - _bossBodyParent.transform.position;
        //    //_bossBodyParent.transform.position += length.normalized * _lastMoveSpeed * Time.deltaTime;

        //    //if (_lastMoveIndex == _lastMovePos.Count - 1)
        //    //{
        //    //    _bossBodyParent.transform.LookAt(_player.transform);
        //    //}
        //    //else
        //    //{
        //    //    //Quaternion q = Quaternion.LookRotation(_bossBodyParent.transform.position + _lastRandomVectorPos);
        //    //    Quaternion q = Quaternion.LookRotation(_lastMovePos[_lastMoveIndex]);
        //    //    _bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 100.0f * Time.deltaTime);
        //    //    //_bossBodyParent.transform.Rotate(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

        //    //    //_bossBodyParent.transform.LookAt(_bossBodyParent.transform.position + _lastRandomVectorPos);
        //    //    //_bossBodyParent.transform.LookAt(_player.transform);
        //    //}

        //    //if (length.magnitude < 1.5f)
        //    //{
        //    //    // タックルをしようとしていたらボイスを流す
        //    //    //if ((10) == _lastMoveIndex)
        //    //    //{
        //    //    //    AudioManager.instance.stopAllVoice();
        //    //    //    AudioManager.instance.playVoice(AudioName.VoiceName.IV12);
        //    //    //}

        //    //    _lastRandomStopTime -= Time.deltaTime;

        //    //    if (_lastRandomStopTime <= 0.0f)
        //    //    {

        //    //        _lastRandomStopTime = _lastRandomStopTimeMax;
        //    //        _lastRandomVectorPos = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 10.0f), Random.Range(-100.0f, 100.0f));

        //    //        ++_lastMoveIndex;

        //    //        if (_lastMoveIndex == _lastMovePos.Count)
        //    //        {
        //    //            //_anim.SetBool("tackle", true);
        //    //            //_anim.Play("Tackle");


        //    //            // 撃ち続けてボイスを流す
        //    //            //AudioManager.instance.stopAllVoice();
        //    //            //AudioManager.instance.playVoice(AudioName.VoiceName.IV13);
        //    //        }
        //    //    }
        //    //}

        //    _bezierTime += Time.deltaTime * _bezierTimeSpeed;
        //    if (_bezierTime >= 1)
        //    {
        //        _bezierTime = 1.0f;
        //    }

        //    var nextPos = _bezier.GetPointAtTime(_bezierTime);
        //    //var length = nextPos - _bossBodyParent.transform.position;
        //    //_bossBodyParent.transform.position = _bezier.GetPointAtTime(_bezierTime);

        //    _bossBodyParent.transform.LookAt(nextPos);
        //    //Quaternion q = Quaternion.LookRotation(nextPos);
        //    //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 3600.0f * Time.deltaTime);

        //    //_bossBodyParent.transform.position += length * Time.deltaTime * 10;
        //    _bossBodyParent.transform.position = nextPos;

        //    if (_bezierTime == 1)
        //    {
        //        _bezierTime = 0;

        //        //if (_lastMoveIndex >= _lastMovePos.Count) { return; }
        //        //_bezier = _bezier = new Bezier(_lastMovePos[_lastMoveIndex], _lastMovePos[_lastMoveIndex + 1], _lastMovePos[_lastMoveIndex + 2], _lastMovePos[_lastMoveIndex + 3]);
        //        //_lastMoveIndex += 4;

        //        var vector = _bossBodyParent.transform.position - _lastMovePos[_lastMoveIndex + 1];
        //        var newPos = _bossBodyParent.transform.position + vector;

        //        _lastMoveIndex += 3;
        //        if (_lastMoveIndex >= _lastMovePos.Count) { return; }
        //        _bezier = new Bezier(_bossBodyParent.transform.position, newPos, _lastMovePos[_lastMoveIndex + 1], _lastMovePos[_lastMoveIndex + 2]);
        //    }         
        //}
        //// タックル前の予備動作
        //else if (_lastBeforeRotateAngle > _LAST_BEFORE_ROTATE_ANGLE_MAX)
        //{
        //    //var length = _lastBeforeTacklePos[_lastBeforeTacklePosIndex] - _bossBodyParent.transform.position;
        //    //_bossBodyParent.transform.position += length * _lastBeforeTackleSpeed * Time.deltaTime;

        //    ////_bossBodyParent.transform.LookAt(_lastBeforeTacklePos[_lastBeforeTacklePosIndex]);

        //    //Quaternion q = Quaternion.LookRotation(_lastBeforeTacklePos[_lastBeforeTacklePosIndex]);
        //    //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 1000.0f * Time.deltaTime);

        //    //if (length.magnitude < 1.5f)
        //    //{
        //    //    ++_lastBeforeTacklePosIndex;

        //    //    if (_lastBeforeTacklePosIndex == _lastBeforeTacklePos.Length)
        //    //    {
        //    //        _anim.Play("Tackle");
        //    //    }
        //    //}

        //    if (_lastBeforeSleepTime > 0)
        //    {
        //        _lastBeforeSleepTime -= Time.deltaTime;
        //        //Quaternion q = Quaternion.LookRotation(_player.transform.position);
        //        //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 1000.0f * Time.deltaTime);
        //        _bossBodyParent.transform.LookAt(_player.transform);

        //        //if (_lastSleepVoiceFlag)
        //        //{
        //        //    AudioManager.instance.stopAllVoice();
        //        //    AudioManager.instance.playVoice(AudioName.VoiceName.IV12);
        //        //    _lastSleepVoiceFlag = false;
        //        //}
        //        _bossBodyParent.transform.Translate(0, -Time.deltaTime * _lastBrforeSleepDownSpeed, 0);

        //        return;
        //    }

        //    _lastBeforeRotateAngle -= _lastBeforeRotateSpeed * Time.deltaTime;

        //    var offsetAngle = _lastBeforeRotateAngle + Mathf.PI / 2;
        //    var offset = new Vector3(0, Mathf.Sin(offsetAngle) * _lastBeforeLengthY, Mathf.Cos(offsetAngle) * _lastBeforeLengthZ);
        //    var nextPos = _lastBeforeTackleSenterPos + offset;

        //    var length = nextPos - _bossBodyParent.transform.position;
        //    _bossBodyParent.transform.position += length * _lastBeforeMoveSpeed * Time.deltaTime;

        //    //_bossBodyParent.transform.LookAt(nextPos);
        //    Quaternion q = Quaternion.LookRotation(nextPos);
        //    _bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 1000.0f * Time.deltaTime);
        //    //transform.Rotate(-_lastBeforeRotateSpeed * Time.deltaTime * Mathf.Rad2Deg, 0, 0);
        //    //_bossBodyParent.transform.Rotate(_lastBeforeRotateAngle * Mathf.Rad2Deg, 0, 0);

        //    //Quaternion q = Quaternion.LookRotation(length);
        //    //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 3600.0f * Time.deltaTime);

        //    if (_lastBeforeRotateAngle <= _LAST_BEFORE_ROTATE_ANGLE_MAX)
        //    {
        //        _anim.Play("Tackle");
        //    }

        //}
        if (_lastPreparationTime > 0.0f)
        {
            _lastPreparationTime -= Time.deltaTime;

            _bossBodyParent.transform.LookAt(_player.transform);
            if (_lastDownRotate < _lastDownRotateMax)
            {
                _lastDownRotate -= Time.deltaTime * _lastDownSpeed;
            }
            _bossBodyParent.transform.Rotate(_lastDownRotate, 0, 0);

            //[SerializeField]
            //Vector3 _lastPreparationPos;
            //[SerializeField]
            //float _lastPreparationAngleSpeed = 1.0f;
            //float _lastPreparationAngle = 0.0f;
            //[SerializeField]
            //float _lastPreparationMoveSpeed = 1.0f;

            //[SerializeField]
            //float _lastStaggerSpeed = 1.0f;
            //float _lastStaggerAngle = 1.0f;

            var pos = _bossBodyParent.transform.position;
            var vector = _lastPreparationRandPos - pos;

            _lastPreparationAngle += _lastPreparationAngleSpeed * Time.deltaTime;

            _lastPreparationAngleSpeed += _lastPreparationAngleAcceleration  * Time.deltaTime;

            pos += vector.normalized * _lastPreparationMoveSpeed * Time.deltaTime;
            pos += Vector3.right * Mathf.Sin(_lastPreparationAngle) * _lastStaggerLength * Time.deltaTime;

            //var length = _lastPreparationPos - pos;
            //_bossBodyParent.transform.position += length.normalized;

            _bossBodyParent.transform.position = pos;
            _bossBodyParent.transform.Rotate(0, 0, Mathf.Sin(_lastPreparationAngle));

            var length = _lastPreparationRandPos - _bossBodyParent.transform.position;
            if (length.magnitude < 0.5f)
            {
                Vector3 lastRandPos = _lastTacklePos + Vector3.forward * _lastRandMoveLength.z + Vector3.up * _lastRandMoveLength.y;
                Vector3 randLength = new Vector3(Random.Range(-_lastRandMoveLength.x, _lastRandMoveLength.x),
                                                                         Random.Range(-_lastRandMoveLength.y, _lastRandMoveLength.y),
                                                                         Random.Range(-_lastRandMoveLength.z, _lastRandMoveLength.z));
                _lastPreparationRandPos = lastRandPos + randLength;

                //_lastMoveIndex;
                //_lastPreparationFlag = true;
            }


            if (_lastPreparationTime < _lastPreparationPoseTime)
            {
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Tackle"))
                {
                    _anim.Play("Tackle");
                }
                
            }
            else
            {
                _lastParticleCreateTime += Time.deltaTime;
                if (_lastParticleCreateTime > _lastParticleInterval)
                {
                    var particle = Instantiate(_hitParticle);
                    var randPos = new Vector3(
                            Random.Range(-_lastParticleRandPos.x, _lastParticleRandPos.x),
                            Random.Range(-_lastParticleRandPos.y, _lastParticleRandPos.y),
                            Random.Range(-_lastParticleRandPos.z, _lastParticleRandPos.z)
                        );
                    particle.transform.position = _bossBodyParent.transform.position + randPos;

                    _lastParticleCreateTime = 0;
                }
            }
        }
        // まだ止まる時間なら
        else if(_lastSleepTime > 0.0f)
        {
            _lastSleepTime -= Time.deltaTime;
            if (_lastSleepTime <= 0.0f)
            {
                //_anim.Play("Tackle");
                AudioManager.instance.play3DSe(gameObject, AudioName.SeName.BossMove);
            }

            _bossBodyParent.transform.LookAt(_player.transform);
            if (_lastDownRotate > 0.0f)
            {
                _lastDownRotate -= Time.deltaTime * _lastDownSpeed;
            }
            _bossBodyParent.transform.Rotate(_lastDownRotate, 0, 0);

            //_lastPreparationAngle += _lastPreparationAngleSpeed * Time.deltaTime;
            //_bossBodyParent.transform.Rotate(0, 0, Mathf.Sin(_lastPreparationAngle));


            var pos = _bossBodyParent.transform.position;
            var vector = _lastPreparationPos - pos;

            _lastPreparationAngle += _lastPreparationAngleSpeed * Time.deltaTime;
            _lastPreparationAngleSpeed += _lastPreparationAngleAcceleration  * Time.deltaTime;

            pos += vector.normalized * _lastPreparationMoveSpeed * Time.deltaTime;
            pos += Vector3.right * Mathf.Sin(_lastPreparationAngle) * _lastStaggerLength * Time.deltaTime;

            _bossBodyParent.transform.position = pos;
            _bossBodyParent.transform.Rotate(0, 0, Mathf.Sin(_lastPreparationAngle));

            //_lastParticleCreateTime += Time.deltaTime;
            //if (_lastParticleCreateTime > _lastParticleInterval)
            //{
            //    var particle = Instantiate(_hitParticle);
            //    var randPos = new Vector3(
            //            Random.Range(-_lastParticleRandPos.x, _lastParticleRandPos.x),
            //            Random.Range(-_lastParticleRandPos.y, _lastParticleRandPos.y),
            //            Random.Range(-_lastParticleRandPos.z, _lastParticleRandPos.z)
            //        );
            //    particle.transform.position = _bossBodyParent.transform.position + randPos;

            //    _lastParticleCreateTime = 0;
            //}
        }
        // プレイヤーへのタックル
        else
        {
            var particle = Instantiate(_hitParticle);
            particle.transform.position = _bossBodyParent.transform.position + Vector3.forward * 3.0f;

            _bossBodyParent.transform.LookAt(_player.transform);

            _lastPreparationAngle += _lastPreparationAngleSpeed * Time.deltaTime;
            _lastPreparationAngleSpeed += _lastPreparationAngleAcceleration  * Time.deltaTime;
            _bossBodyParent.transform.Rotate(0, 0, Mathf.Sin(_lastPreparationAngle));

            if (!_lastTackleFlag)
            {
                // 撃ち続けてボイスを流す
                AudioManager.instance.stopAllVoice();
                AudioManager.instance.playVoice(AudioName.VoiceName.IV13);
            }

            if (_lastSlowStartTime <= 0)
            {
                // スローじゃなかったらスローにする
                if (!SlowMotion._instance.isSlow)
                {
                    // タックルフラグをオンに
                    _lastTackleFlag = true;

                    SlowMotion._instance.GameSpeed(0.1f);
                    SlowMotion._instance.isLimit = false;
                    SlowMotion._instance.limiterFlag = true;    // リミッターフラグ解除
                }
            }
            else
            {
                _lastSlowStartTime -= Time.deltaTime;
            }

            var length = _player.transform.position - _bossBodyParent.transform.position;
            _bossBodyParent.transform.position += length * _lastTackleSpeed * Time.deltaTime;

            //Quaternion q = Quaternion.LookRotation(_player.transform.position);
            //_bossBodyParent.transform.rotation = Quaternion.RotateTowards(_bossBodyParent.transform.rotation, q, 1000.0f * Time.deltaTime);

            _bossBodyParent.transform.LookAt(_player.transform);
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
            SlowMotion._instance.limiterFlag = false;

            // タックルフラグを切る
            _lastTackleFlag = false;

            // 弾がボスに当たらないようにする
            Physics.IgnoreLayerCollision(LayerName.Bullet, LayerName.Boss, true);
        }

        //if (!_rigidbody.useGravity)
        //{
        //    _rigidbody.useGravity = true;
        //    _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        //    var vector = _bossBodyParent.transform.position - _player.transform.position;
        //    _rigidbody.velocity = vector.normalized * _climaxVelocity;
        //}

        //if (!_rigidbody.useGravity)
        //{
        //    _rigidbody.useGravity = true;
        //    _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        //    var vector = _bossBodyParent.transform.position - _player.transform.position;
        //    _rigidbody.AddForce(/*vector.normalized * _climaxVelocity + */Vector3.up * _climaxUpPower,ForceMode.Impulse);
        //}

        if (_climaxUpPower > 0)
        {
            var vector = _bossBodyParent.transform.position - _player.transform.position;
            var vector2D = new Vector3(vector.x, 0, vector.z);
            _bossBodyParent.transform.position += (vector2D.normalized * _climaxVelocity + Vector3.up * _climaxUpPower) * Time.deltaTime;
            _climaxUpPower -= Time.deltaTime;
            _bossBodyParent.transform.Rotate(-Time.deltaTime * 180.0f, -Time.deltaTime * 180.0f, -Time.deltaTime * 180.0f);
        }

        _lastParticleCreateTime += Time.deltaTime;
        if (_lastParticleCreateTime > 0.1f)
        {
            var particle = Instantiate(_hitParticle);
            var randPos = new Vector3(
                    Random.Range(-_lastParticleRandPos.x, _lastParticleRandPos.x),
                    Random.Range(-_lastParticleRandPos.y, _lastParticleRandPos.y),
                    Random.Range(-_lastParticleRandPos.z, _lastParticleRandPos.z)
                );
            particle.transform.position = _bossBodyParent.transform.position + randPos;

            // ボスのパーツ
            for (int i = 0; i < _climaxFirstDebrisNum; ++i)
            {
                var obj = Instantiate(_climaxDebrisPrefab);
                obj.transform.position = _bossBodyParent.transform.position + randPos;
                obj.transform.rotation = transform.rotation;
                obj.transform.localScale = Vector3.one * _climaxFirstDebrisSize;
            }

            _lastParticleCreateTime = 0;
        }

        _climaxTime += Time.deltaTime;
        if(/*_climaxDestroyTime < _climaxTime*/ _climaxUpPower <= 0)
        {
            Damage();

            // HP が 0 になったら死ぬ
            if (_hp <= 0)
            {
                // ボスのパーツ
                for (int i = 0; i < _climaxLastDebrisNum; ++i)
                {
                    var obj = Instantiate(_climaxDebrisPrefab);
                    obj.transform.position = _bossBodyParent.transform.position;
                    obj.transform.rotation = transform.rotation;
                    obj.transform.localScale = Vector3.one * _climaxLastDebrisSize;
                }

                // 爆発エフェクト
                var effect = Instantiate(_climaxExplosion);
                effect.transform.position = _bossBodyParent.transform.position;
                effect.transform.position = Vector3.zero;

                Destroy(gameObject);
                GameDirector.instance.isBossDestroy();
            }
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    // クライマックスなら
    //    if (_state == BossState.CLIMAX)
    //    {
    //        if (col.gameObject.tag == TagName.Floor)
    //        {
    //            if (_climaxBoundNum > 0)
    //            {
    //                // バウンドの回数を減らす
    //                _climaxBoundNum--;

    //                // ２次元での向きを出す
    //                var bossPos = new Vector2(_bossBodyParent.transform.position.x, _bossBodyParent.transform.position.z);
    //                var playerPos = new Vector2(_player.transform.position.x, _player.transform.position.z);
    //                var vector2 = bossPos - playerPos;

    //                // ３次元に変換
    //                var vector = new Vector3(vector2.x, 0.0f, vector2.y);

    //                // その方向に飛ばす
    //                _rigidbody.velocity = vector.normalized * _climaxBoundPowerSide;
    //                _rigidbody.velocity += Vector3.up * _climaxBoundPowerUp;

    //                // バウンドの力を半分に
    //                _climaxBoundPowerSide *= 0.5f;
    //                _climaxBoundPowerUp *= 0.5f;

    //                // 最後のバウンドの時滑らせるために強い力をあたえる
    //                if (_climaxBoundNum == 0)
    //                {
    //                    _rigidbody.velocity += vector.normalized * _climaxVelocity;

    //                    //_anim.SetBool("slide", true);
    //                    _anim.Play("Slide");
    //                }

    //                // バウンドエフェクトを出す
    //                var boundParticle = Instantiate(_boundParticle);
    //                boundParticle.transform.position = _effectPos.transform.position;
    //            }
    //            else
    //            {
    //                // 滑るを出す
    //                var boundParticle = Instantiate(_slideParticle);
    //                boundParticle.transform.position = _effectPos.transform.position;
    //                boundParticle.transform.LookAt(_player.transform);
    //                boundParticle.transform.parent = transform;
    //            }
    //        }
    //        return;
    //    }
    //}

    void EffectAndDamage(Collider col)
    {
        if (_lastTackleFlag)
        {
            var particle = Instantiate(_hitLastParticle);
            particle.transform.position = col.transform.position;
        }
        else
        {
            //var particle = Instantiate(_hitParticle);
            var particle = Instantiate(_hitLastParticle);
            particle.transform.position = col.transform.position;
        }
            
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

        // パージ音
        AudioManager.instance.play3DSe(gameObject, AudioName.SeName.BossPurge);
    }

    void EmissionColorChange(Color color1, Color color2, float time, float timeMax = 1.0f)
    {
        float r = (time / timeMax);
        float sin = r;
        float cos = timeMax - r;
        foreach (var mat in _mat)
        {
            Color color = (color1 * sin) + (color2 * cos);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color);
        }
    }

    //Vector3 Bezier(float count, Vector3[] pos)
    //{
    //    float x = (1 - count) * (1 - count) * pos[0].x
    //              + 2 * (1 - count) * count * pos[1].x
    //                        + count * count * pos[2].x;
    //    float y = (1 - count) * (1 - count) * pos[0].y
    //              + 2 * (1 - count) * count * pos[1].y
    //                        + count * count * pos[2].y;
    //    float z = (1 - count) * (1 - count) * pos[0].z
    //              + 2 * (1 - count) * count * pos[1].z
    //                        + count * count * pos[2].z;
    //    return new Vector3(x, y, z);
    //}
}

//[System.Serializable]
//public class Bezier : System.Object
//{
//    public Vector3 p0;
//    public Vector3 p1;
//    public Vector3 p2;
//    public Vector3 p3;

//    public float ti = 0f;

//    private Vector3 b0 = Vector3.zero;
//    private Vector3 b1 = Vector3.zero;
//    private Vector3 b2 = Vector3.zero;
//    private Vector3 b3 = Vector3.zero;

//    private float Ax;
//    private float Ay;
//    private float Az;

//    private float Bx;
//    private float By;
//    private float Bz;

//    private float Cx;
//    private float Cy;
//    private float Cz;

//    // Init function v0 = 1st point, v1 = handle of the 1st point , v2 = handle of the 2nd point, v3 = 2nd point
//    // handle1 = v0 + v1
//    // handle2 = v3 + v2
//    public Bezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
//    {
//        this.p0 = v0;
//        this.p1 = v1;
//        this.p2 = v2;
//        this.p3 = v3;
//    }
//    // 0.0 >= t <= 1.0
//    public Vector3 GetPointAtTime(float t)
//    {
//        this.CheckConstant();
//        float t2 = t * t;
//        float t3 = t * t * t;
//        float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + p0.x;
//        float y = this.Ay * t3 + this.By * t2 + this.Cy * t + p0.y;
//        float z = this.Az * t3 + this.Bz * t2 + this.Cz * t + p0.z;
//        return new Vector3(x, y, z);
//    }

//    private void SetConstant()
//    {
//        this.Cx = 3f * ((this.p0.x + this.p1.x) - this.p0.x);
//        this.Bx = 3f * ((this.p3.x + this.p2.x) - (this.p0.x + this.p1.x)) - this.Cx;
//        this.Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;
//        this.Cy = 3f * ((this.p0.y + this.p1.y) - this.p0.y);
//        this.By = 3f * ((this.p3.y + this.p2.y) - (this.p0.y + this.p1.y)) - this.Cy;
//        this.Ay = this.p3.y - this.p0.y - this.Cy - this.By;

//        this.Cz = 3f * ((this.p0.z + this.p1.z) - this.p0.z);
//        this.Bz = 3f * ((this.p3.z + this.p2.z) - (this.p0.z + this.p1.z)) - this.Cz;
//        this.Az = this.p3.z - this.p0.z - this.Cz - this.Bz;
//    }

//    // Check if p0, p1, p2 or p3 have change
//    private void CheckConstant()
//    {
//        if (this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.p3 != this.b3)
//        {
//            this.SetConstant();
//            this.b0 = this.p0;
//            this.b1 = this.p1;
//            this.b2 = this.p2;
//            this.b3 = this.p3;
//        }
//    }
//}
