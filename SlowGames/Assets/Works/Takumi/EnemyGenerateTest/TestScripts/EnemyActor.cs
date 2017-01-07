using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//test: えねみーの動きを管理.
public class EnemyActor : MonoBehaviour
{
 
    //速度,攻撃頻度等を参照する
    Enemy _enemy;

    [SerializeField]
    Animator _enemyAnimator;

    enum AnimationState
    {
        instruition    = 0,
        Fighting       = 1,
        JustBeforeShot = 2,
        Shot           = 3,
        Shoted         = 4,

    }

    enum ActionType
    {

        //MoveToGeneratePos,
        FisrtAction     = 0,
        TargetRun       = 1,
        ProvocationMove = 2,
        Stay            = 3,
        Shot            = 4,
        Fall            = 5,
        SinMove         = 6,
        Takkle          = 7
   
    }

    [SerializeField]
    ActionType _currentAction;
    UnityEngine.AI.NavMeshAgent _navimesh;

    [SerializeField,Range(0,100)]
    float _playerToMaxDistance = 10;

    //エネミーが向かう方向
    public Transform _currentTarget;
    Vector3 _basePosition;
    bool    _isHitToEnemy;
    Vector3 _provMoveTarget;

    //player情報
    GameObject _playerTransform;

    //
    bool _isShot = false;
    int _stayCount = 0;

    //実行関数
    Dictionary<ActionType,System.Action> _actionDic = new Dictionary<ActionType, System.Action>();

    void Awake()
    {
        _enemy = this.gameObject.GetComponent<Enemy>();
        _navimesh = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        _actionDic = new Dictionary<ActionType, System.Action>();

        _actionDic.Add(ActionType.FisrtAction,FirstAction);
        _actionDic.Add(ActionType.TargetRun,TargetRun);
        _actionDic.Add(ActionType.ProvocationMove,ProvocationMove);
        _actionDic.Add(ActionType.Stay,Stay);
        _actionDic.Add(ActionType.Shot,Shot);
        _actionDic.Add(ActionType.Fall,DownRespawn);
        _actionDic.Add(ActionType.SinMove,SinMove);
        _actionDic.Add(ActionType.Takkle,Tackle);
       
    }


    void Start()
    {

        //_currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
        _basePosition = _currentTarget.position;
        _isShot = false;
        _isHitToEnemy = false;
        _playerTransform = GameObject.FindGameObjectWithTag(TagName.Player);
        _enemyAnimator.SetInteger("ActionType",(int)AnimationState.instruition);
       

    }

    void Update()
    {
        //stateに合わせて、関数を実行
        _actionDic[_currentAction]();
        //test:常に、プレイヤーをみるようようにしてる.違和感を感じたら変更


    }

    //生成
    void FirstAction()
    {

        if (_currentTarget == null)
        {
            //うまくいってなかったら生成仕直し
            this.gameObject.GetComponent<Enemy>().SilentDestroy();
            return;
        }
        else
        {
            //しっかりターゲットがきまってたら移動開始
            _currentAction = ActionType.TargetRun;
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        } 

    }

    //
    void ChangeAction(ActionType action,float activeTime = 0)
    {
        _currentAction = action;
        _enemy._activeCounter = activeTime;
    }


    //プレイヤーとの距離が引数以上かを返す
    bool CheckPlayerToDistance(float maxDistance)
    {

        Vector3 distance = _playerTransform.transform.position - transform.position;

        //一定距離離れてたら
        if (Mathf.Abs(distance.magnitude) > maxDistance)
        {
            return true;
        }

        return false;
    }


    //ターゲットにむかって走り続けます
    void TargetRun()
    {
         //ターゲットに向かって走る
        _navimesh.SetDestination(_basePosition);
        transform.LookAt(_playerTransform.transform.position);

    }

    //挑発するように、横に移動します
    void ProvocationMove()
    {

      //横移動
        if (_enemy._activeCounter > 0)
      {
          _enemy._activeCounter -= Time.deltaTime;
      }
      //リセット
      else
      {
            
          _provMoveTarget = new Vector3(Random.Range(-_enemy.info.sideMoveRange,_enemy.info.sideMoveRange),
                                        0,
                                       (Random.Range(-_enemy.info.sideMoveRange,_enemy.info.sideMoveRange)));
          var targetPosition = _basePosition + _provMoveTarget;

                //float activeTime = RandomActiveTime(1);
          float activeTime = _enemy.info.activeTimeMax;
          iTween.MoveTo (gameObject, iTween.Hash ("x", targetPosition.x,"z", targetPosition.z,
                                                  "time",activeTime,"easeType",iTween.EaseType.linear));

          StartCoroutine(RotateEnemy(activeTime,targetPosition));

          ChangeAction(ActionType.Stay,activeTime);

      }

    }

    //本来行くはずの逆の方向に煽り移動をします（エネミー同士がぶつかった際
    void ReProvocationMove(float activeTime)
    {
        //
        iTween.Stop(this.gameObject,"move");

        _provMoveTarget *= -1;
        var targetposition = _basePosition + _provMoveTarget * (activeTime / _enemy.info.activeTimeMax);
    
        iTween.MoveTo (gameObject, iTween.Hash ("x", targetposition.x,"z", targetposition.z,
                                                "time",activeTime,"easeType",iTween.EaseType.linear));
    }


    //FixMe : 動く方向に傾ける
    IEnumerator RotateEnemy(float activeTime, Vector3 targetPosition)
    {
        
//
//        //
//        var add = (targetPosition.magnitude - transform.position.magnitude);
//        transform.LookAt(_playerTransform.transform.position);
//
//        //正面のベクトル
//        float z = 2 * Vector3.forward.x;
//        float x = z * 0.5f;
//        float _x = -z * 2;
//        float _z = 0.5f * x;
//
//        Vector3 dir = new Vector3(_x,_z, transform.position.y);
//        float angle = 0.13f;
//
//        //左方向だったら 
//        angle *= dir.magnitude > 0 ? 1 : -1 ;
//        float count = activeTime * 0.5f;
//
//        while (count > 0)
//        {
//            count -= Time.deltaTime;    
//            transform.Rotate(dir,angle);
//            yield return null;
//        }
//
//        count = activeTime * 0.5f;
//        angle *= -1;
//
//        while (count > 0)
//        {
//            count -= Time.deltaTime;
//            transform.Rotate(dir,angle);
//
//            yield return null;
//        }
//
        yield return null;
    }


  


    //待機状態,与えられたactiviTime分移動を止める
    void Stay()
    {

        if (_enemy._activeCounter > 0)
        {   
            //Stay中にエネミー同士があたったら
//            if (_isHitToEnemy)
//            {
//                //移動する方向を変える
//                ReProvocationMove(_enemy._activeCounter);
//                _isHitToEnemy = false;
//            }
            _enemy._activeCounter -= Time.deltaTime;
           
          
        }
        else
        {
            _stayCount += 1;
            transform.LookAt(_playerTransform.transform.position);

            if (_stayCount > _enemy.info.shotFrequency)
            {
                ChangeAction(ActionType.Shot);
                _stayCount = 0;
            }
            else
            {
                //射撃をしない場合煽り行動をもう一度を行います
                ChangeAction(ActionType.ProvocationMove);
            }

        }

    }

    void Shot()
    {   
        //コルーチンで管理
        if (!_isShot)
        {
            _isShot = true;
            //移動中に敵同士がぶつかって蓄積された重力加速を０にしておく（射撃時に意図しない方向に移動してしまうため）
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            StartCoroutine(ShotMotion());
        }

    }

    [SerializeField]
    float _justShotTime = 0.0f;

    IEnumerator ShotMotion()
    {

        float timeCount = 0;

        //撃つ数
        int shotCount = _enemy.info.chamberValue;
        //連弾する時の遅延時間
        float shotDelayTime = _enemy.info.shotDelay;

        //アニメーションを撃つかまえに
        _enemyAnimator.SetInteger("ActionType", (int)AnimationState.JustBeforeShot);

        //test; wait for Second がうまく行かない代わり
        timeCount = _justShotTime;
        while (timeCount > 0)
        {
            timeCount -= Time.deltaTime; 
            yield return null;
        }

        for (int i = 0; i < shotCount; i++)
        {
            //撃つアニメーション
            //shotラグ//test: アニメーションとのズレを緩和
            _enemyAnimator.SetInteger("ActionType", (int)AnimationState.Shot);
            timeCount = 0.15f;
            while (timeCount > 0)
            {
                timeCount -= Time.deltaTime; 
                yield return null;
            }

            //実際に射撃
            //:test サウンド 
            gameObject.GetComponentInChildren<EnemyShot>().DoShot();
            AudioManager.instance.play3DSe(gameObject,AudioName.SeName.Thunder);

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
                _enemyAnimator.SetInteger("ActionType", (int)AnimationState.JustBeforeShot);

                //shotラグ//test; wait for Second がうまく行かない代わり
                timeCount = _justShotTime;
                while (timeCount > 0)
                {
                    timeCount -= Time.deltaTime; 
                    yield return null;
                }

            }

        }

        _isShot = false;
        //stayに遷移
        ChangeAction(ActionType.Stay);
        _enemyAnimator.SetInteger("ActionType",(int)AnimationState.Fighting);


    }

    //アニメーションする時間をあらかじめ指定した範囲のランダムで決める
    float  RandomActiveTime(float min = 0.0f)
    {
        return Random.Range(min,_enemy.info.activeTimeMax);
    }


    void OnTriggerEnter(Collider other)
    {
        //ナビメッシュの効果がある際に判定
        if (_navimesh.enabled == true)
        {

            if (_enemy.doFall)//上から出現するなら
            {
                ChangeAction(ActionType.Fall, 0);

            }
            else//した階からの出現なら
            {   
                if (_enemy.Type == EnemyType.Tackle)
                {
                    ChangeAction(ActionType.SinMove, 0);
                    Debug.Log("タックル");
                }
                else
                {
                    //玉のうつひんどを上げるためでてきたらまず撃つ
                    ChangeAction(ActionType.Shot, RandomActiveTime());
                }
            }

            //ナビメッシュの移動をやめる
            _navimesh.enabled = false;
            _enemyAnimator.SetInteger("ActionType",(int)AnimationState.Fighting);

        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == TagName.Enemy)
        {
            _isHitToEnemy = true;
        }
    }

    //落ちるリスポーン
    bool _isFalling = true;
    void DownRespawn()
    {
        if (_isFalling)
        {
            _isFalling = false;
            StartCoroutine(Fall());
        }
        else
        {
            transform.LookAt(_playerTransform.transform.position);
        }
    }


    [SerializeField]
    float _downLengthMax = 20;//30
    [SerializeField]
    Vector2 _upRangeMinMax = new Vector2(5,13);//30


    //落ちて登場
    IEnumerator Fall()
    {

        ///落とす
        //落ちる角度を調整
        int randomAngle = UnityEngine.Random.Range(265, 275);
        float x = Mathf.Cos(ToRadian(randomAngle));
        float y = Mathf.Sin(ToRadian(randomAngle));
        Vector3 randomDirec = new Vector3(x, y, 0);

        var direction = randomDirec * _downLengthMax;
        var target = (direction + transform.position);

        iTween.MoveTo(gameObject, iTween.Hash("position", target, "time", 1.0f, "easeType", iTween.EaseType.easeOutCirc));

        yield return new WaitForSeconds(1.0f);

        //ターゲットを更新
        target = transform.position;

        ///あげる
        //あげる距離をランダムで調整
        int randomUpRange = Random.Range((int)_upRangeMinMax.x, (int)_upRangeMinMax.y);
        target += Vector3.up * randomUpRange;

//        //エネミーの位置を調整
//        enemy.transform.position = transform.position + new Vector3(x,0,z);

        iTween.MoveTo(gameObject, iTween.Hash("position", target, "time", 1.0f, "easeType", iTween.EaseType.easeOutBack));

        yield return new WaitForSeconds(1.0f);

        _basePosition = transform.position;

        //タックルタイプはタックル
        if (_enemy.Type == EnemyType.Tackle)
        {
            ChangeAction(ActionType.SinMove,0);
        }
        else
        {
            ChangeAction(ActionType.Shot, RandomActiveTime());
        }
        yield return null;

    }

    [SerializeField]
    float _sideMoveSpeed = 200;
    [SerializeField]
    float _sideMoveLenfth = 10;

    [SerializeField,Range(1,20)]
    float _tackleRange = 7;
    [SerializeField]
    float _tackleSpeed = 10;
    [SerializeField,Range(0,5)]
    float _tackleChargeTime = 1;


    //Test:横移動しながら前に進み
    void SinMove()
    {
        _enemy._activeCounter += Time.deltaTime * _sideMoveSpeed;
        Vector3 side = transform.right * (Mathf.Cos(ToRadian(_enemy._activeCounter)) * _sideMoveLenfth);
        transform.position += (transform.forward + side) * Time.deltaTime;
        transform.LookAt(_playerTransform.transform.position);
        //プレイヤーと
        if (!CheckPlayerToDistance(_tackleRange))
        {
            ChangeAction(ActionType.Takkle,_tackleChargeTime);

        }

    }

    void Tackle()
    {
        //チャージ
        if (_enemy._activeCounter > 0)
        {
           _enemy._activeCounter -= Time.deltaTime;
           transform.LookAt(_playerTransform.transform.position);
        }
        else
        {   
           // タックル
           transform.position += (transform.forward) * Time.deltaTime * _tackleSpeed;
        }
         
    }

    static float ToRadian(float value)
    {
        return value * 3.14f / 180.0f;
    }
}
