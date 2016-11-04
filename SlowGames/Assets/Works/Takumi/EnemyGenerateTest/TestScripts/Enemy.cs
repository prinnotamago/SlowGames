using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _deathEffect;

    //Transform _targetPostion;
    //多重Hitを避ける
    bool death = false;

    [System.Serializable]
    struct EnemyAttackInfo
    {
        public float moveSpeed;   //移動速度,移動時間,
        public float stayTimeMax;   //待機時間
        public float sideMoveRange; //横移動の幅
        public float activeCounter;    //行動中(攻撃前)のカウントをする用
        public float activeTimeMax;    //行動数の限界.
        public int   shotFrequency;      //何回にどのくらい撃つかの頻度.
        public int   chamberValue;       //何発連続で撃つか
        public float shotDelay;     ////連続で撃つ時の遅延時間
          
    }

    [SerializeField]
    List<EnemyAttackInfo> _enemyAttackInfos;
    EnemyAttackInfo _enemyInfo;

    public TargetPosition  _generatePostion;

    void Start()
    {
        death = false;

        int waveCount = GenerateManager.GetCurrentWave();

        //それ以上のデータがない場合,最大の設定を入れる
        if (waveCount >= _enemyAttackInfos.Count)
        {
            waveCount = _enemyAttackInfos.Count - 1;
        }

       _enemyInfo = _enemyAttackInfos[waveCount];

       _moveSpeed = _enemyInfo.moveSpeed;


    }

    //たまにあたったら死にます
    void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.tag == TagName.Bullet)
        {
            if (death)
            {
                return;
            }
    
            death = true;
            
            //エフェクト
            var effect = Instantiate(_deathEffect);
            effect.transform.position = transform.position;
            //音
            //AudioManager.instance.play3DSe(effect,AudioName.SeName.Thunder);
            //死ぬ
            FindObjectOfType<GenerateManager>().AddDeathCount(_generatePostion);
            //Test:スコア
            ScoreManager.instance.AddHitEnemyCount();
            Destroy(this.gameObject);
        }
    }

    //スコアを更新せず静かに殺します 
    public void SilentDestroy()
    {
        FindObjectOfType<GenerateManager>().AddDeathCount(_generatePostion);
        Destroy(this.gameObject);
    }

    //移動速度,移動時間,
    [SerializeField,Range(0,100)]
    public float _moveSpeed   = 3.0f;

    //待機時間
    [SerializeField,Range(0,5)]
    public float _stayTimeMax = 1.0f;

    //横移動の幅
    [SerializeField,Range(0,10)]
    public float _sideMoveRange = 8.0f;

    //行動中のカウントをする用
    public float _activeCounter = 0;
    [SerializeField,Range(0,5)]
    public float _activeTimeMax = 2;

    //何回にどのくらい撃つかの頻度.
    public int _shotFrequency = 3;

    //何発連続で撃つか
    public int _chamberValue = 1;
 
    //連続で撃つ時の遅延時間
    [SerializeField]
    public float _shotDelay = 1.0f;




  
}
