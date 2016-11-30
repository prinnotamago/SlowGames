using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyInfos : MonoBehaviour {

    static SwordEnemyInfos _instance;

    static public SwordEnemyInfos instace
    {
        get{return _instance;}

    }

    [SerializeField]
    List<SwordEnemyMover.SwordEnemyData> _easyDates;
    [SerializeField]
    List<SwordEnemyMover.SwordEnemyData> _normalDates;
    [SerializeField]
    List<SwordEnemyMover.SwordEnemyData> _hardDates;

    static Dictionary<SwordEnemyType,List<SwordEnemyMover.SwordEnemyData>> swordEnemyDic =
           new Dictionary<SwordEnemyType, List<SwordEnemyMover.SwordEnemyData>>();

    public void Start()
    {
        swordEnemyDic.Add(SwordEnemyType.Easy,_easyDates);
        swordEnemyDic.Add(SwordEnemyType.Normal,_normalDates);
        swordEnemyDic.Add(SwordEnemyType.Hard,_hardDates);


    }

    //エネミーのタイプ、またウェーブ数にあわせて情報を返します
    public SwordEnemyMover.SwordEnemyData GetEnemyData(SwordEnemyType type,int waveCount)
    {
       var enemyDataList = swordEnemyDic[type];

       //ウェーブ数にあわせたエネミーのデータがなければ、一番強いデータをとりあえず渡しておく
       int count = enemyDataList.Count <= waveCount ? enemyDataList.Count - 1 : waveCount;

       return enemyDataList[count];

    
    }

              

    public void Awake()
    {
        _instance = this;
    }


}
