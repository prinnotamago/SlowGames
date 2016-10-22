using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//test: えねみーくんをプレイヤーに向かわせます
public class EnemyActor : MonoBehaviour
{

    enum ActionType
    {
        TargetRun,
        ProvocationMove,
        Stay,
    }
    [SerializeField]
    ActionType _currentAction;
    ActionType _nextAction;

    NavMeshAgent _navimesh;

    [SerializeField,Range(0,100)]
    float _playerToMaxDistance = 10;


    //移動速度,移動時間,
    [SerializeField,Range(0,100)]
    float _moveSpeed   = 3.0f;
    float _sideMoveSpeed = 3.0f;
    [SerializeField,Range(0,100)]
    float _sideMoveSpeedMax = 1.0f;

    [SerializeField,Range(0,5)]
    float _moveTimeMax = 1.0f;

    //待機時間
    [SerializeField,Range(0,5)]
    float _stayTimeMax = 1.0f;

    //横移動の幅
    [SerializeField,Range(0,10)]
    float _sideMoveRange = 10.0f;

    //行動中のカウントをする際にしようします
    float _activeCounter = 0;

    //エネミーが向かう方向
    public Transform _currentTarget;
    GameObject _playerTransform;

    Dictionary<ActionType,System.Action> _actionDic = new Dictionary<ActionType, System.Action>();

    void Awake()
    {

        _navimesh = this.gameObject.GetComponent<NavMeshAgent>();

    }


    void Start()
    {

        //_currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
        _actionDic = new Dictionary<ActionType, System.Action>();
       
        _actionDic.Add(ActionType.TargetRun,TargetRun);
        _actionDic.Add(ActionType.ProvocationMove,ProvocationMove);
        _actionDic.Add(ActionType.Stay,Stay);

        _playerTransform = GameObject.FindGameObjectWithTag("Player");
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


        _actionDic[_currentAction]();
        transform.LookAt(_playerTransform.transform.position);

    }

    void ChangeAction(ActionType action,float activeTime = 1)
    {
        _currentAction = action;
        _activeCounter = activeTime;
        
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
        if (_activeCounter > 0)
        {
            _activeCounter -= Time.deltaTime;
            transform.Translate(Vector3.left * _sideMoveSpeed * Time.deltaTime);
        }
        //リセット
        else
        {
            //移動時間,移動速度をランダムに生成
            _sideMoveSpeed = _sideMoveSpeedMax - Random.Range(0,(_sideMoveSpeedMax * 2));
            //_activeCounter = Random.Range(0.0f, _moveTimeMax);

            ChangeAction(ActionType.Stay,1);

        }

    }

    //
    void Stay()
    {
        if (_activeCounter > 0)
        {
            _activeCounter -= Time.deltaTime;

        }
        else
        {
            //stay前に決めておいた動きをします
            ChangeAction(ActionType.ProvocationMove);
        }
        

    }


    void OnTriggerEnter(Collider other)
    {

        ChangeAction(ActionType.ProvocationMove,1);
        gameObject.GetComponentInChildren<EnemyShot>()._isShotStart = true;
        _navimesh.enabled = false;

    }
}
