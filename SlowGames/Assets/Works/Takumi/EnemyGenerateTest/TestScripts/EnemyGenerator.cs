using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    Easy,
    Normal,
    Tackle,

    Last,
}


public enum TargetPosition
{

   Left = 0,
   Front = 1,
   Right = 2,

   UpLeft  = 3,
   UpFront = 4,
   UpRight = 5,

   Last,
  
}

//enemyを生成
public class EnemyGenerator : MonoBehaviour
{
    
    //雑魚敵
    [SerializeField]
    List<GameObject> _easyEnemys = new List<GameObject>();
    [SerializeField]
    List<GameObject> _normalEnemys = new List<GameObject>();
    [SerializeField]
    List<GameObject> _hardEnemys = new List<GameObject>();


    [SerializeField]
    List<Transform> _generateList = new List<Transform>();

    [SerializeField]
    Transform _left;
    [SerializeField]
    Transform _front;
    [SerializeField]
    Transform _right;

    [SerializeField]
    Transform _upLeft;
    [SerializeField]
    Transform _upFront;
    [SerializeField]
    Transform _upRight;

    [SerializeField]
    Transform _generateLeft;
    [SerializeField]
    Transform _generateFront;
    [SerializeField]
    Transform _generateRight;

    [SerializeField]
    Transform _generateUpLeft;
    [SerializeField]
    Transform _generateUpFront;
    [SerializeField]
    Transform _generateUpRight;



    //生成するエネミーを記憶 
    Dictionary<EnemyType,List<GameObject>> _enemysDic = new Dictionary<EnemyType, List<GameObject>>();
    public Dictionary<TargetPosition,Transform> _targetPositionDic = new Dictionary<TargetPosition,Transform>();
    public Dictionary<TargetPosition,Transform> _generatePositionDic = new Dictionary<TargetPosition,Transform>();

    void Awake()
    {
        
        _enemysDic.Add(EnemyType.Easy, _easyEnemys);
        _enemysDic.Add(EnemyType.Normal, _normalEnemys);
        _enemysDic.Add(EnemyType.Tackle, _hardEnemys);

        _targetPositionDic.Add(TargetPosition.Left,_left);
        _targetPositionDic.Add(TargetPosition.Right,_right);
        _targetPositionDic.Add(TargetPosition.Front,_front);
        _targetPositionDic.Add(TargetPosition.UpLeft,_upLeft);
        _targetPositionDic.Add(TargetPosition.UpRight,_upRight);
        _targetPositionDic.Add(TargetPosition.UpFront,_upFront);

        _generatePositionDic.Add(TargetPosition.Left,_generateLeft);
        _generatePositionDic.Add(TargetPosition.Right,_generateRight);
        _generatePositionDic.Add(TargetPosition.Front,_generateFront);
        _generatePositionDic.Add(TargetPosition.UpLeft,_generateUpLeft);
        _generatePositionDic.Add(TargetPosition.UpRight,_generateUpRight);
        _generatePositionDic.Add(TargetPosition.UpFront,_generateUpFront);
    }


    //ランダムに,生成位置を取得する
    public  TargetPosition GetRandomGeneratePos(int[] generateCount, int  enemyLimit = 2)
    {
        //生成可能な,配列番号を記憶する
        List<int> canGeneratePos = new List<int>();

        for (int i = 0; i < generateCount.Length; i++)
        {   
            ////敵キャラがいない、または生成上限に達していない場所だったら.
            if (generateCount[i] <= (enemyLimit - 1))
            {
                //敵キャラがいない、または生成上限に達していない場所の、配列番号を記憶
                canGeneratePos.Add(i);
            }
        }

        //もし生成可能な場所が一つもなければ Lastを返す
        if (canGeneratePos.Count == 0)
        {
            Debug.Log("生成可能な場所がないため TargetPosition.Lastを返してます");
            return TargetPosition.Last;
        }

        //生成可能場所からランダムに選ぶ
        int random = Random.Range(0,canGeneratePos.Count);

        return (TargetPosition)(canGeneratePos[random]);

    }

    //ランダムに,地上の生成位置を取得する
    public  TargetPosition GetRandomGroundGeneratePos()
    {
        int random = Random.Range(0,3);
        return (TargetPosition)random;
    }

    //ランダムに,空中の生成位置を取得する
    public  TargetPosition GetRandomSkyGeneratePos()
    {
        int random = Random.Range(3,(int)TargetPosition.Last);
        return (TargetPosition)random;
    }
   

    //generatorの生成位置に敵キャラを配置
    public void GenerateEnemy(EnemyType enemyType, TargetPosition generatePosition  = TargetPosition.Front)
    {    

        //選んだエネミータイプから,ランダムでpatternを選び生成.
        var enemyList = _enemysDic[enemyType];
        int random = Random.Range(0, (enemyList.Count));

        //生成
        GameObject enemy = enemyList[random];
        Instantiate(enemy);

        //todo: 出現位置からランダムに生成
        var setTransform = _generatePositionDic[generatePosition];
        enemy.transform.position = setTransform.position;
        enemy.transform.rotation = setTransform.rotation;

        //自分がどこに生成された的なのかをキヲクさせる
        enemy.GetComponent<Enemy>()._generatePostion = generatePosition;
        enemy.GetComponent<EnemyActor>()._currentTarget = _targetPositionDic[generatePosition];

    }

    //
    public void GenerateEnemy(ref List<EnemyType> enemyTypes, TargetPosition generatePosition  = TargetPosition.Front)
    {    

        //生成可能なenemyのTypeからランダムでどれかを生成する
        int randomType = Random.Range(0,enemyTypes.Count);
        var type = enemyTypes[randomType];

        //選んだエネミータイプから,ランダムでパターンを選び生成.:FixMe:ここは任意で決められた方がいいかも？
        var enemyList = _enemysDic[type];
        int random = Random.Range(0, (enemyList.Count));

        //生成
        GameObject enemy = enemyList[random];
        Instantiate(enemy);

        //todo: 出現位置からランダムに生成
        var setTransform = _generatePositionDic[generatePosition];
        enemy.transform.position = setTransform.position;
        enemy.transform.rotation = setTransform.rotation;

        //自分がどこに生成された的なのかをキヲクさせる
        enemy.GetComponent<Enemy>()._generatePostion = generatePosition;
        enemy.GetComponent<EnemyActor>()._currentTarget = _targetPositionDic[generatePosition];

    }

    // 

}