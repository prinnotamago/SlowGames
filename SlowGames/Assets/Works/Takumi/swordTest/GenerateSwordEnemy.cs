using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SwordEnemyType
{
    Easy,
    Normal,
    Hard

}

public class GenerateSwordEnemy : MonoBehaviour {

    [SerializeField]
    SwordEnemyMover _swordEnemy;

   
    //波状攻撃のデータ構造体です
    [System.Serializable]
    struct SwordWaveData
    {
        //敵の死亡数がこの値以上の場合以下の情報で敵を出現させます.
        public int _startDieCount;

        public int _generateTimingCount;     //シーン内にいるエネミーが残り何体再度出現させるか
        public int _generateCount;           //何体ずつだすか
        public List<SwordEnemyType> _generateTypeList;//どのタイプを出すか
        public List<SlashSword.SlashPattern> _weekPointList;

            
        public SwordWaveData(int startDieCount,
                             int generateTimingCount,int generateCount,
                             List<SwordEnemyType> generateTypeList,List<SlashSword.SlashPattern> weekPointList)
        {

           _startDieCount = startDieCount;
       
           _generateTimingCount =  generateTimingCount;
           _generateCount       = generateCount;

           _generateTypeList    = new List<SwordEnemyType>();
           _weekPointList       = new List<SlashSword.SlashPattern>();
           _generateTypeList    = generateTypeList;
           _weekPointList       = weekPointList; 

        }    
    
    }

    [SerializeField]
    List<SwordWaveData> _waveDataList = new List<SwordWaveData>();

    int _waveCount = 0;
    int _killCount = 0;//testよう
    [SerializeField]
    List<SwordEnemyMover.SwordEnemyData> _enemyData;

    [SerializeField]
    List<float> _generateAngles = new List<float>();
    List<int> _genereatePosCount = new List<int>();

    [SerializeField]
    float _generateDistance = 10.0f;

    void Generate(int generatePosNumber)
    {

        var enemy = Instantiate(_swordEnemy.gameObject);

        //ウェーブ数にあわせたエネミーのデータがなければ、一番強いデータをとりあえず渡しておく
        int waveCount = _waveCount <= _enemyData.Count ?  _waveCount : (_enemyData.Count - 1);


        //生成場所を設定、場所を記憶,数を更新
//        int  generatePosNumber =  GetRandomGeneratePos(_genereatePosCount);// Random.Range(0,_generateAngles.Count);
//        
//        _genereatePosCount[generatePosNumber] += 1;

        //生成可能なenemyのTypeからランダムでどれかを生成する
        int randomType = Random.Range(0,_waveDataList[waveCount]._generateTypeList.Count);
        int randomWeek = Random.Range(0,_waveDataList[waveCount]._weekPointList.Count);

        var type = _waveDataList[waveCount]._generateTypeList[randomType];
        var week = _waveDataList[waveCount]._weekPointList[randomWeek];

        //生成
        SwordEnemyMover.SwordEnemyData enemyData = SwordEnemyInfos.instace.GetEnemyData(type,waveCount);
        enemyData.enemyPattern = week;
//      SlashSword.SlashPattern  weekPoint = GetRandomSlashPattern();
//      enemyData.generatePosNumber = generatePosNumber;
//      enemyData.enemyPattern = week;

        //エネミーのステータスを更新
        enemy.GetComponent<SwordEnemyMover>().setState(enemyData);

        //生成位置を計算.//FixMe:生成毎に計算が無駄
        float randomAngle = _generateAngles[generatePosNumber];
        float x = _generateDistance * Mathf.Cos(ToRadian(randomAngle));
        float z = _generateDistance * Mathf.Sin(ToRadian(randomAngle));

        //エネミーの位置を調整
        enemy.transform.position = transform.position + new Vector3(x,0,z);

    }



    IEnumerator DelayGenerate(int count, List<int> generatePosNumbers, float delayCount = 0.5f)
    {
        float counter = delayCount;

        for (int i = 0; i < count; ++i)
        {
            Generate(generatePosNumbers[i]);

               while(counter > 0)
               {
                    counter -= Time.deltaTime;
                    yield return null;

               }

               counter = delayCount;
        }

        yield return null;
        
    }



    public void UpdateEnemyCount(int generatePosNumber)
    {
        //同じところからは生成しない.
        _genereatePosCount[generatePosNumber] -= 1;
        _killCount += 1;

        //ウェーブの更新チェッ
        if (_waveCount < _waveDataList.Count - 1)
        {

            //敵の死亡数が一定数行っていたらウェーブを更新
            if (_killCount > _waveDataList[(_waveCount + 1)]._startDieCount)
            {
                _waveCount += 1;
            }

        }

        var waveData = _waveDataList[_waveCount];

        //エネミーの残り数が一定の数
        int liveEnemysCount = GetLiveEnemyCount();

        if (liveEnemysCount <= waveData._generateTimingCount)
        {
            
            // 生成する前に男体出してるかを更新
            List<int> randomValues = new List<int>();
            int generateCount = waveData._generateCount;
            for (int i = 0; i < generateCount; i++)
            {
                randomValues.Add(GetRandomGeneratePos(_genereatePosCount));
                _genereatePosCount[randomValues[i]] += 1;
            }


            //ウェーブに合わせた、数をだすぞ
            StartCoroutine(DelayGenerate(generateCount,randomValues));
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

        //Generate(GetRandomGeneratePos(_genereatePosCount));

        for (int i = 0; i < 2; i++)
        {
            int posNumber = (GetRandomGeneratePos(_genereatePosCount));
            _genereatePosCount[posNumber] += 1;
            Generate(posNumber);
        }
       
	}

    SlashSword.SlashPattern GetRandomSlashPattern()
    {
        int randomPattern = Random.Range((int)SlashSword.SlashPattern.UP_DOWN,(int)SlashSword.SlashPattern.ALL_RANGE);    

        return (SlashSword.SlashPattern)randomPattern;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate(GetRandomGeneratePos(_genereatePosCount));
        }
	}
    

    //
    static float ToRadian(float value)
    {
        return value * 3.14f / 180.0f;
    }
}
