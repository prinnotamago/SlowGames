using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    Easy,
    Normal,
    Hard,

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

    //生成するエネミーを記憶 
    Dictionary<EnemyType,List<GameObject>> _enemysDic = new Dictionary<EnemyType, List<GameObject>>();


    void Start()
    {

        _enemysDic.Add(EnemyType.Easy, _easyEnemys);
        _enemysDic.Add(EnemyType.Normal, _normalEnemys);
        _enemysDic.Add(EnemyType.Hard, _hardEnemys);

    }

    [SerializeField]
    EnemyType _testGenerateType;

    void Update()
    {

        //test: ”G”Keyで生成
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateEnemy(_testGenerateType);    
        }

    }

    //test:とりあえず敵を生成
    public void GenerateEnemy(EnemyType enemyType)
    {    

        //選んだエネミータイプから,ランダムでpatternを選び生成.
        var enemyList = _enemysDic[enemyType];
        int random = Random.Range(0, (enemyList.Count));

        //生成
        GameObject enemy = enemyList[random];
        Instantiate(enemy);

        //test: ジェネレーターの場所を基準に生成
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;

    }

}