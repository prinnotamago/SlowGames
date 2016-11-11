using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//波状攻撃のデータ構造体です
[System.Serializable]
public struct WaveData
{
    //敵の死亡数がこの値以上の場合以下の情報で敵を出現させます.
    public int _startDieCount;

    public int _generateTimingCount;     //シーン内にいるエネミーが残り何体再度出現させるか
    public int _generateCount;           //何体ずつだすか
    public int _enemyLimit;                  //同時出現数の限界値
    public List<EnemyType> _generateTypeList;//どのタイプを出すか

    public WaveData(int startDieCount,
                    int generateTimingCount,int generateCount,
                    int enemyLimit,List<EnemyType> generateTypeList)
    {

       _startDieCount = startDieCount;
       
       _generateTimingCount =  generateTimingCount;
       _generateCount       = generateCount;
       _generateTypeList    = new List<EnemyType>();
       _enemyLimit          = enemyLimit;
       _generateTypeList    =  generateTypeList;
        
    }

}

//
public class GenerateManager : MonoBehaviour
{

    EnemyGenerator _enemyGenerator;

    //生成した数を管理
    int[] _currentEnemysCount = new int[((int)TargetPosition.Last)];
   
    //同じ場所に何体まで出せるか(全体数の限界値ではありません)
    [SerializeField,Range(1,20)]
    int _enemyLimit = 1;

    [SerializeField]
    List<WaveData> _waveDate = new List<WaveData>();

    static int _currentWaveCount = 0;
    public static int GetCurrentWave()
    {
         return _currentWaveCount;
    }




    //死亡数をカウントします
    int _deathCount = 0;

    void Start()
    {
        //初期化
        _enemyGenerator = this.gameObject.GetComponent<EnemyGenerator>();
        _deathCount = 0;

        //開幕３体配置.
        _currentWaveCount = 0;
//        var waveData = _waveDate[_currentWaveCount];
//        SetEnemy(1,waveData._generateTypeList);

        TutorialSet();
    }

    void TutorialSet()
    {

        List<TargetPosition> genePos = new List<TargetPosition>();

        genePos.Add(TargetPosition.Left);
        genePos.Add(TargetPosition.Right);
        genePos.Add(TargetPosition.UpFront);

        //チュートリアル用の三体を生成
        for (int i = 0; i < genePos.Count; i++)
        {
            DefaultSetEnemy(EnemyType.Easy,genePos[i]);
        }

    }


    //固定位置に生成
    void DefaultSetEnemy(EnemyType enemyType = EnemyType.Easy,TargetPosition targetPosition = TargetPosition.Left)
    {
            //生成した場所のカウントを覚えておく
            _currentEnemysCount[(int)targetPosition] += 1;
            //生成
            _enemyGenerator.GenerateEnemy(EnemyType.Easy, targetPosition);    


                                        
    }

    void Update()
    {

        
        //test: ”G”Keyで生成
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SetEnemy();

            string debugCount = "";
            foreach (var count in _currentEnemysCount)
            {
                debugCount += count;
            }
            Debug.Log(debugCount);

        }

    }

    //死んだ回数を記憶
    public void AddDeathCount(TargetPosition generatePosition)
    {
        //死んだ数を死んだを更新
        _deathCount += 1;
        _currentEnemysCount[(int)generatePosition] -= 1;
        UpdateEnemyCount();
    }



    //生きてるエネミーの総数を取得
    public int GetLiveEnemyCount()
    {
        int currentEnemyCount = 0;
        string sousu = "";
        foreach (var count in _currentEnemysCount)
         {
                 currentEnemyCount += count;
                 sousu += count.ToString();
         }
       //  Debug.Log(sousu);
         return currentEnemyCount;
    }


    //FixMe://２体死ぬごとに、敵キャラを生成
    void UpdateEnemyCount()
    {

        var waveData = _waveDate[_currentWaveCount];

        //ウェーブのデータが最大にいったらそれ以上はいかない
        if (_currentWaveCount  < _waveDate.Count - 1)
        {

            //敵の死亡数が一定数行っていたらウェーブを更新
            if (_deathCount > _waveDate[(_currentWaveCount + 1)]._startDieCount)
            {
                _currentWaveCount += 1;
            }

        }
        //死ぬごとに、敵キャラを生成
        int liveEnemysCount = GetLiveEnemyCount();

        if (liveEnemysCount <= waveData._generateTimingCount)
        {
            SetEnemy(waveData._generateCount,waveData._generateTypeList);
        }

    }

//    //敵を生成する場所の数、　またそこから出す敵のカウントを設定、敵キャラを配置]
//    //*敵キャラがいる場合は生成させない
//    void SetEnemy(int count = 1,EnemyType enemyType = EnemyType.Easy)
//    {
//
//        StartCoroutine(DelayGenerate(enemyType,count,0.5f));
//
//
////        for (int i = 0; i < count; i++)
////        {
////        
////            //地上の出現位置をランダムに取得 ,//そこに敵キャラが一定以上いたら、再取得
////            TargetPosition generatePosition = _enemyGenerator.GetRandomGeneratePos(_currentEnemysCount, 5);
////
////            //位置を示さないものが帰ってきたら処理しない
////            //なおこれはよくない処理です
////            if (generatePosition == TargetPosition.Last)
////            {
////                Debug.Log("生成できませんでした。");
////                return;
////            }
////
////            //生成した場所のカウントを覚えておく
////            _currentEnemysCount[(int)generatePosition] += 1;
////
////            //生成
////            _enemyGenerator.GenerateEnemy(EnemyType.Easy, generatePosition);
////        }
//                                
//    }
//
//    //一気に生成させない
//    IEnumerator DelayGenerate(EnemyType enemyType, int count = 1, float delayTime = 0.5f)
//    {
//
//        float counter = 0;
//        
//        for (int i = 0; i < count; i++)
//        {
//
//            //地上の出現位置をランダムに取得 ,//そこに敵キャラが一定以上いたら、再取得
//            TargetPosition generatePosition = _enemyGenerator.GetRandomGeneratePos(_currentEnemysCount, _enemyLimit);
//
//            //位置を示さないものが帰ってきたら処理しない
//            //なおこれはよくない処理です
//            if (generatePosition == TargetPosition.Last)
//            {
//                Debug.Log("生成できませんでした。");
//
//                break;
//            }
//
//            //生成した場所のカウントを覚えておく
//            _currentEnemysCount[(int)generatePosition] += 1;
//
//            //生成
//            _enemyGenerator.GenerateEnemy(EnemyType.Easy, generatePosition);
//
//            counter = delayTime;
//
//            yield return null;
//
//            while (true)
//            {
//                counter -= Time.deltaTime;
//
//                if (counter < 0)
//                {
//                  break;
//                }
//
//                yield return null;
//            }
//
//            yield return null;
//        }
//
//
//    }

    //敵を生成する場所の数、　またそこから出す敵のカウントを設定、敵キャラを配置]
    //*敵キャラがいる場合は生成させない
    void SetEnemy(int count,List<EnemyType> enemyTypes)
    {

        StartCoroutine(DelayGenerate(enemyTypes,count,0.5f));
    }

    //一気に生成させない
    IEnumerator DelayGenerate(List<EnemyType> enemyTypes, int count = 1, float delayTime = 0.5f)
    {

        float counter = 0;
        
        for (int i = 0; i < count; i++)
        {

            //地上の出現位置をランダムに取得 ,//そこに敵キャラが一定以上いたら、再取得
            TargetPosition generatePosition = _enemyGenerator.GetRandomGeneratePos(_currentEnemysCount, _enemyLimit);

            //位置を示さないものが帰ってきたら処理しない
            //なおこれはよくない処理です
            if (generatePosition == TargetPosition.Last)
            {
                Debug.Log("生成できませんでした。");

                break;
            }

            //生成した場所のカウントを覚えておく
            _currentEnemysCount[(int)generatePosition] += 1;

            //生成
            _enemyGenerator.GenerateEnemy(ref enemyTypes, generatePosition);

            counter = delayTime;

            yield return null;

            while (true)
            {
                counter -= Time.deltaTime;

                if (counter < 0)
                {
                  break;
                }

                yield return null;
            }

            yield return null;
        }


    }
  
}
