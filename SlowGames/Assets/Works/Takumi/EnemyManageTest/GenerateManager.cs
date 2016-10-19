using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateManager : MonoBehaviour
{
   
    //生成した数を管理
    int[] _genrateCounts = new int[((int)GeneratePosition.Last)];

    EnemyGenerator _enemyGenerator;

    void Start()
    {
        //初期化
        _enemyGenerator = this.gameObject.GetComponent<EnemyGenerator>();

    }

    void Update()
    {
        //test: ”G”Keyで生成
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetGroundEnemy();
        }

    }


    //敵を生成する場所の数、　またそこから出す敵のカウントを設定、敵キャラを配置]
    //*敵キャラがいる場合は生成させない
    void SetGroundEnemy(bool isGround = true, bool isSky = true)
    {
        //現在地上で、敵キャラが配置されてないところを取得
        GeneratePosition generatePosition = _enemyGenerator.GetRandomGeneratePos();
        //生成した場所のカウントを覚えておく
        _genrateCounts[(int)generatePosition] += 1;

       
       //生成
       _enemyGenerator.GenerateEnemy(EnemyType.Easy,generatePosition);

                         
    }


  
}
