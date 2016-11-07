using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum SwordEnemyType
{
    L_Sholder,
    L_Leg,
    L_Arm,

    R_Sholder,
    R_Leg,
    R_Arm,


    Last,

}

public class GenerateSwordEnemy : MonoBehaviour {
    //波状攻撃のデータ構造体です
    [System.Serializable]
    public struct SwordWaveData
    {
        //敵の死亡数がこの値以上の場合以下の情報で敵を出現させます.
        public int _startDieCount;

        public int _generateTimingCount;     //シーン内にいるエネミーが残り何体再度出現させるか
        public int _generateCount;           //何体ずつだすか
        //public List<SwordEnemyType> _generateTypeList;//どのタイプを出すか

        public SwordWaveData(int startDieCount,
                        int generateTimingCount,int generateCount)
                       // List<SwordEnemyType> generateTypeList)
        {

           _startDieCount = startDieCount;
       
           _generateTimingCount =  generateTimingCount;
           _generateCount       = generateCount;

         //  _generateTypeList    = new List<SwordEnemyType>();
         //  _generateTypeList    =  generateTypeList;

        }    
    
    }

    [SerializeField]
    List<SwordWaveData> _waveDataList = new List<SwordWaveData>();

    int _waveCount = 0;
    int _killCount = 0;//testよう
 
    [SerializeField]
    SwordEnemyMover _swordEnemy;

    [SerializeField]
    List<SwordEnemyMover.SwordEnemyData> _enemyData;

    [SerializeField]
    List<float> _generateAngles = new List<float>();
    List<int> _genereatePosCount = new List<int>();

    [SerializeField]
    float _generateDistance = 10.0f;

    void Generate()
    {

        var enemy = Instantiate(_swordEnemy.gameObject);

        int waveCount = _waveCount <= _enemyData.Count ?  _waveCount : (_enemyData.Count - 1);


        //生成場所を設定、場所を記憶,数を更新
        int  generatePosNumber =  GetRandomGeneratePos(_genereatePosCount);// Random.Range(0,_generateAngles.Count);
        float randomAngle = _generateAngles[generatePosNumber];
        _genereatePosCount[generatePosNumber] += 1;

        //生成位置をエネミー側にも記憶.
        SwordEnemyMover.SwordEnemyData enemyData = _enemyData[waveCount];
        enemyData.generatePosNumber = generatePosNumber;

        //エネミーのステータスを更新
        enemy.GetComponent<SwordEnemyMover>().setState(enemyData);


        //生成位置を計算.//FixMe:生成毎に計算が無駄
        float x = _generateDistance * Mathf.Cos(ToRadian(randomAngle));
        float z = _generateDistance * Mathf.Sin(ToRadian(randomAngle));

        //エネミーの位置を調整
        enemy.transform.position = transform.position + new Vector3(x,0,z);

    }



    public void UpdateEnemyCount(int generatePosNumber)
    {
        //同じところからは生成しない.
        _genereatePosCount[generatePosNumber] -= 1;
        _killCount += 1;


        var waveData = _waveDataList[_waveCount];

        //ウェーブのデータが最大にいったらそれ以上はいかない
        if (_waveCount  < _waveDataList.Count - 1)
        {

            //敵の死亡数が一定数行っていたらウェーブを更新
            if (_killCount > _waveDataList[(_waveCount + 1)]._startDieCount)
            {
                _waveCount += 1;
            }

        }

        //エネミーの残り数が一定の数
        int liveEnemysCount = GetLiveEnemyCount();

        if (liveEnemysCount <= waveData._generateTimingCount)
        {
            Generate();
        }


    }

    //ランダムに,生成位置を取得する
    public  int GetRandomGeneratePos(List<int> generateCount)
    {
        //生成可能な,配列番号を記憶する
        List<int> canGeneratePos = new List<int>();

        for (int i = 0; i < generateCount.Count; i++)
        {   
            ////敵キャラがいったいもいなければ
            if (generateCount[i] <= 1)
            {
                //敵キャラがいない、または生成上限に達していない場所の、配列番号を記憶
                canGeneratePos.Add(i);
            }
        }

        //もし生成可能な場所が一つもなければ Lastを返す
        if (canGeneratePos.Count == 0)
        {
            Debug.Log("生成可能な場所がないため TargetPosition.Lastを返してます");
            return -1;
        }

        //生成可能場所からランダムに選ぶ
        int random = Random.Range(0,canGeneratePos.Count);

        return (canGeneratePos[random]);

    }



    //生きてるエネミーの総数を取得
    public int GetLiveEnemyCount()
    {
        int currentEnemyCount = 0;
        foreach (var count in _genereatePosCount)
        {
                 currentEnemyCount += count;
        }
                return currentEnemyCount;
    }


	// Use this for initialization
	void Start()
    {
        _waveCount = 0;
        for (int i = 0; i < _generateAngles.Count; i++)
        {
            _genereatePosCount.Add(0);
        }

        Generate();
	}


    float count = 0;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            
           Generate();

        }

       

	}
    

    //
    float ToRadian(float value)
    {
        return value * 3.14f / 180.0f;
    }
}
