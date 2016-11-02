using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//test: えねみーの動きを管理.
public class EnemyActor : MonoBehaviour
{
    //
//    //移動速度,移動時間,
//    [SerializeField,Range(0,100)]
//    float _moveSpeed   = 3.0f;
//    float _sideMoveSpeed = 3.0f;
//    [SerializeField,Range(0,100)]
//    float _sideMoveSpeedMax = 1.0f;
//
//    [SerializeField,Range(0,5)]
//    float _moveTimeMax = 1.0f;
//
//    //待機時間
//    [SerializeField,Range(0,5)]
//    float _stayTimeMax = 1.0f;
//
//    //横移動の幅
//    [SerializeField,Range(0,10)]
//    float _sideMoveRange = 8.0f;
//
//    //行動中のカウントをする際にしようします
//    float _activeCounter = 0;
//    [SerializeField,Range(0,5)]
//    float _activeTimeMax = 2;

    //速度,攻撃頻度等を参照する
    Enemy _enemy;

    enum ActionType
    {

        //MoveToGeneratePos,
        TargetRun,
        ProvocationMove,
        Stay,
        Shot,
    }

    [SerializeField]
    ActionType _currentAction;
    UnityEngine.AI.NavMeshAgent _navimesh;

    [SerializeField,Range(0,100)]
    float _playerToMaxDistance = 10;

    //エネミーが向かう方向
    public Transform _currentTarget;
    //player情報
    GameObject _playerTransform;

    //
    bool _isShot = false;

    //実行関数
    Dictionary<ActionType,System.Action> _actionDic = new Dictionary<ActionType, System.Action>();

    void Awake()
    {
        _enemy = this.gameObject.GetComponent<Enemy>();
        _navimesh = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

    }


    void Start()
    {

        //_currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
        _isShot = false;

        _actionDic = new Dictionary<ActionType, System.Action>();

        _actionDic.Add(ActionType.TargetRun,TargetRun);
        _actionDic.Add(ActionType.ProvocationMove,ProvocationMove);
        _actionDic.Add(ActionType.Stay,Stay);
        _actionDic.Add(ActionType.Shot,Shot);

        _playerTransform = GameObject.FindGameObjectWithTag(TagName.Player);

    }

    void Update()
    {

//        if (_activeCounter <= 0)
//        {
//
//            //位置が一定以上はなれてたら追いかける
//            if (CheckPlayerToDistance(_playerToMaxDistance))
//            {
//                _currentAction = ActionType.TargetRun;
//            }
//            else
//            {
//                _currentAction = ActionType.ProvocationMove;
//            }
//
//        }

        //stateに合わせて、関数を実行
        _actionDic[_currentAction]();
        //test:常に、プレイヤーをみるようようにしてる.違和感を感じたら変更
        transform.LookAt(_playerTransform.transform.position);

    }

    void ChangeAction(ActionType action,float activeTime = 1)
    {
        _currentAction = action;
        _enemy._activeCounter = activeTime;

    }


    //プレイヤーとの距離が引数以上かを返す
    bool CheckPlayerToDistance(float maxDistance)
    {

        Vector3 distance = _currentTarget.position - transform.position;

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
        _navimesh.SetDestination(_currentTarget.position);

        transform.LookAt(_playerTransform.transform.position);

        //移動方向を計算,取得
//       Vector3 movedirection = (_currentTarget.position - transform.position).normalized;
//        if (CheckPlayerToDistance(_playerToMaxDistance))
//        {
//            _currentAction = ActionType.ProvocationMove;
//        }
//        transform.Translate(movedirection * _moveSpeed * Time.deltaTime);

    }

    //挑発するように、横に移動します
    void ProvocationMove()
    {

      //横移動
        if (_enemy._activeCounter > 0)
      {
          _enemy._activeCounter -= Time.deltaTime;
          //transform.Translate(Vector3.left * _sideMoveSpeed * Time.deltaTime);
      }
      //リセット
      else
      {
          //移動時間,移動速度をランダムに生成
          //_sideMoveSpeed = _sideMoveSpeedMax - Random.Range(0,(_sideMoveSpeedMax * 2));
          //_activeCounter = Random.Range(0.0f, _moveTimeMax);

          var targetPosition = _currentTarget.position 
                + new Vector3(Random.Range(-_enemy._sideMoveRange,_enemy._sideMoveRange),
                             0,
                             (Random.Range(-_enemy._sideMoveRange,_enemy._sideMoveRange)));

          float activeTime = RandomActiveTime(1);
          iTween.MoveTo (gameObject, iTween.Hash ("position", targetPosition, "time",activeTime));

          ChangeAction(ActionType.Stay,activeTime);

      }

    }

    //待機状態,与えられたactiviTime分移動を止める

    int _stayCount = 0;

    void Stay()
    {

        if (_enemy._activeCounter > 0)
        {
            _enemy._activeCounter -= Time.deltaTime;

        }
        else
        {
            _stayCount += 1;

            if (_stayCount > _enemy._shotFrequency)
            {
                ChangeAction(ActionType.Shot);
                _stayCount = 0;
            }
            else
            {
                //stay前に決めておいた動きをします
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
            StartCoroutine(ShotMotion());
        }

    }


    float _justShotTime = 0.4f;

    IEnumerator ShotMotion()
    {
        //撃つ数
        int shotCount = _enemy._chamberValue;
        //連弾する時の遅延時間
        float shotDelayTime = _enemy._shotDelay;

        //アニメーションを撃つかまえに
        //
        //   未実装
        //
        ////////////////////////
        yield return new WaitForSeconds(_justShotTime);

        for (int i = 0; i < shotCount; i++)
        {
            //撃つ
            gameObject.GetComponentInChildren<EnemyShot>().Shot();

            if (shotCount > 1)
            {   
                //１発目以降は間隔を開けて撃つ
                yield return new WaitForSeconds(shotDelayTime);
            }

        }

        _isShot = false;
        //stayに遷移
        ChangeAction(ActionType.Stay);
    }
   

    float  RandomActiveTime(float min = 0.0f)
    {
        return Random.Range(min,_enemy._activeTimeMax);
    }

    void OnTriggerEnter(Collider other)
    {

        ChangeAction(ActionType.ProvocationMove,RandomActiveTime());
        //目的位置に達したら攻撃を開始する.
        gameObject.GetComponentInChildren<EnemyShot>()._isShotStart = true;
        _navimesh.enabled = false;

    }
}
