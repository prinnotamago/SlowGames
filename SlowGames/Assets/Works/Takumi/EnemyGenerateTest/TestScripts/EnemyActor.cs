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
        instruition = 0,
        Fighting = 1,
        JustBeforeShot = 2,
        Shot = 3,
        Shoted = 4,

    }

    enum ActionType
    {

        //MoveToGeneratePos,
        FisrtAction = 0,
        TargetRun = 1,
        ProvocationMove = 2,
        Stay = 3,
        Shot = 4,
   
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
       
    }


    void Start()
    {

        //_currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
        _isShot = false;
        _playerTransform = GameObject.FindGameObjectWithTag(TagName.Player);
        _enemyAnimator.SetInteger("ActionType",(int)AnimationState.instruition);

    }

    void Update()
    {
        //stateに合わせて、関数を実行
        _actionDic[_currentAction]();
        //test:常に、プレイヤーをみるようようにしてる.違和感を感じたら変更
        transform.LookAt(_playerTransform.transform.position);

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

    [SerializeField]
    float _justShotTime = 0.0f;

    IEnumerator ShotMotion()
    {

        float timeCount = 0;

        //撃つ数
        int shotCount = _enemy._chamberValue;
        //連弾する時の遅延時間
        float shotDelayTime = _enemy._shotDelay;

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
            //撃つ
            _enemyAnimator.SetInteger("ActionType", (int)AnimationState.Shot);
            //shotラグ//test; wait for Second がうまく行かない代わり
            timeCount = 0.15f;

            while (timeCount > 0)
            {
                timeCount -= Time.deltaTime; 
                yield return null;
            }

            gameObject.GetComponentInChildren<EnemyShot>().Shot();
          
            //二発以上かつ最後の弾じゃなければ
            if (shotCount > 1 && (shotCount - 1) > i)
            {   
                //１発目以降は間隔を開けて撃つ
                //yield return new WaitForSeconds(shotDelayTime);
                //shotラグ//test; wait for Second がうまく行かない代わり
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
   

    float  RandomActiveTime(float min = 0.0f)
    {
        return Random.Range(min,_enemy._activeTimeMax);
    }

    void OnTriggerEnter(Collider other)
    {

        if (_navimesh.enabled == true)
        {
            _stayCount = Random.Range(0, (_enemy._shotFrequency + 1)); //撃つ頻度は最初のみランダムに
            //ランダムでしょっぱなうつ
            if (_stayCount >= _enemy._shotFrequency)
            {
                ChangeAction(ActionType.Shot, RandomActiveTime());
            }
            else
            {
                ChangeAction(ActionType.ProvocationMove, RandomActiveTime());
            }
            _navimesh.enabled = false;

            _enemyAnimator.SetInteger("ActionType",(int)AnimationState.Fighting);
        }

    }
}
