using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateManager : MonoBehaviour
{
   
    //生成した数を管理
    int[] _currentEnemysCount = new int[((int)TargetPosition.Last)];

    EnemyGenerator _enemyGenerator;

    //死亡数をカウントします
    int _deathCount = 0;

    public void AddDeathCount(TargetPosition generatePosition)
    {
        //死んだ数を死んだを更新
        _deathCount += 1;
        _currentEnemysCount[(int)generatePosition] -= 1;
        UpdateEnemyCount();
    }

    //FixMe://２体死ぬごとに、敵キャラを生成
    void UpdateEnemyCount()
    {
        //２体死ぬごとに、敵キャラを生成
        if (_deathCount % 2 == 0)
        {
                SetEnemy(2);
        }
    }


    void Start()
    {
        //初期化
        _enemyGenerator = this.gameObject.GetComponent<EnemyGenerator>();
        _deathCount = 0;

        SetEnemy(3);

    }




    void Update()
    {
        //test: ”G”Keyで生成
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetEnemy();

            string debugCount = "";
            foreach (var count in _currentEnemysCount)
            {
                debugCount += count;
            }
            Debug.Log(debugCount);

        }

    }


    //敵を生成する場所の数、　またそこから出す敵のカウントを設定、敵キャラを配置]
    //*敵キャラがいる場合は生成させない
    void SetEnemy(int count = 1, bool isGround = true, bool isSky = true)
    {

        StartCoroutine(DelayGenerate(count,0.5f));


//        for (int i = 0; i < count; i++)
//        {
//        
//            //地上の出現位置をランダムに取得 ,//そこに敵キャラが一定以上いたら、再取得
//            TargetPosition generatePosition = _enemyGenerator.GetRandomGeneratePos(_currentEnemysCount, 5);
//
//            //位置を示さないものが帰ってきたら処理しない
//            //なおこれはよくない処理です
//            if (generatePosition == TargetPosition.Last)
//            {
//                Debug.Log("生成できませんでした。");
//                return;
//            }
//
//            //生成した場所のカウントを覚えておく
//            _currentEnemysCount[(int)generatePosition] += 1;
//
//            //生成
//            _enemyGenerator.GenerateEnemy(EnemyType.Easy, generatePosition);
//        }
                                
    }

    //一気に生成させない
    IEnumerator DelayGenerate(int count = 1, float delayTime = 0.5f)
    {

        float counter = 0;
        
        for (int i = 0; i < count; i++)
        {

            //地上の出現位置をランダムに取得 ,//そこに敵キャラが一定以上いたら、再取得
            TargetPosition generatePosition = _enemyGenerator.GetRandomGeneratePos(_currentEnemysCount, 1);

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
            _enemyGenerator.GenerateEnemy(EnemyType.Easy, generatePosition);

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
