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
    public List<RareEnemy> _rareEnemyInfo;
   

    public WaveData(int startDieCount,
                    int generateTimingCount,int generateCount,
                    int enemyLimit,List<EnemyType> generateTypeList,
                    List<RareEnemy> rareEnemy)
    {

       _startDieCount = startDieCount;
       
       _generateTimingCount =  generateTimingCount;
       _generateCount       =  generateCount;
       _generateTypeList    =  new List<EnemyType>();
       _enemyLimit          =  enemyLimit;
       _generateTypeList    =  generateTypeList;
       _rareEnemyInfo       =  new List<RareEnemy>();
       _rareEnemyInfo       =  rareEnemy;
       
    }

}

[System.Serializable]
public struct RareEnemy
{
    public EnemyType type;
    public int       generateTiming;
}


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
    List<int> _rareEnemyCount = new List<int>();
    static int _currentWaveCount = 0;

    [SerializeField,Range(0,100)]
    int _MAX_ENEMY = 30;

    public static int GetCurrentWave()
    {
         return _currentWaveCount;
    }

    //死亡数をカウントします
    public int _deathCount = 0;

    void Start()
    {
        //初期化
        _enemyGenerator = this.gameObject.GetComponent<EnemyGenerator>();
        for (int i = 0; i < _waveDate.Count; i++)
        {
            _rareEnemyCount.Add(0);
        }

        _deathCount = 0;

        //開幕３体配置.
        _currentWaveCount = 0;
//        var waveData = _waveDate[_currentWaveCount];
//        SetEnemy(1,waveData._generateTypeList);

        if (_isTutorial)
        {
            TutorialSet();
        }
    }

    [SerializeField]
    bool _isTutorial = false;

    //チュートリアル
    public bool isTutorial
    {
        set
        {
            _isTutorial = value;
            if (value)
            {
                TutorialSet();     
            }
                
        }

        get{ return _isTutorial;}

    }


    public void TutorialSet()
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

    public void GameStartSet()
    {
        List<TargetPosition> genePos = new List<TargetPosition>();

        genePos.Add(TargetPosition.Left);
        genePos.Add(TargetPosition.Right);
        genePos.Add(TargetPosition.UpFront);

        //ゲームスタート用の三体を生成
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


    //敵キャラを生成
    void UpdateEnemyCount()
    {

        var waveData = _waveDate[_currentWaveCount];

        //ウェーブのデータが最大にいったらそれ以上はいかない
        if (_currentWaveCount < _waveDate.Count - 1)
        {

            //敵の死亡数が一定数行っていたらウェーブを更新
            if (_deathCount > _waveDate[(_currentWaveCount + 1)]._startDieCount)
            {
                _currentWaveCount += 1;
            }

        }

        //現在出ているレアタイプの数
        int rareEnemyCount = _rareEnemyCount[_currentWaveCount];

        //最大数でてたら通らない
        if (waveData._rareEnemyInfo.Count > rareEnemyCount)
        {
            //タイミングに合わせて、ホーミングタイプのキャラを出す
            if (_deathCount >= waveData._rareEnemyInfo[rareEnemyCount].generateTiming)
            {

                SetEnemy(1, waveData._rareEnemyInfo[rareEnemyCount].type);
                _rareEnemyCount[_currentWaveCount] += 1;


            }
        }

        //死ぬごとに、敵キャラを生成
        int liveEnemysCount = GetLiveEnemyCount();

        if (liveEnemysCount <= waveData._generateTimingCount)
        {

            //限界値以上出さない
            if (_MAX_ENEMY < _deathCount + waveData._generateCount)
            {
                int lastCount = _MAX_ENEMY - _deathCount;
                SetEnemy(lastCount, waveData._generateTypeList);

            }
            else
            {
                SetEnemy(waveData._generateCount, waveData._generateTypeList);
            }
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
    void SetEnemy(int count,EnemyType enemyType)
    {

        StartCoroutine(DelayGenerate(enemyType,count,0.5f));
    }


    //一気に生成させない
    IEnumerator DelayGenerate(EnemyType enemyType, int count = 1, float delayTime = 0.5f)
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
            _enemyGenerator.GenerateEnemy(enemyType, generatePosition);

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

    public void DestroyAllEnemy()
    {

        //List<Enemy> enemys = new List<Enemy>();
        var enemys = GameObject.FindObjectsOfType<Enemy>();

        foreach (var enemy in enemys)
        {
            Destroy(enemy.gameObject);
        }
    }


}

