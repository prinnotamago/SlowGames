using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    private int _score = 0;

    private int _killedEnemyCount = 0;
    private int _shotCount = 0;

    private float _lifeTimeCount = 0.0f;

    private int _flipEnemyBulletCount = 0;

    private int _inpactDamageCount = 0;

    //List<CSVData> _data = null;

    [System.Serializable] //Gun用
    public struct GunEnemyData
    {
        public EnemyType type;
        public int score;
    }

    [System.Serializable]//Sword用
    public struct SwordEnemyData
    {
        public SlashSword.SlashPattern _type;
        public int score;
    }

    public GunEnemyData[] _gunData = null;
    public SwordEnemyData[] _swordData = null;

    /// <summary>
    /// インスタンスを所得
    /// </summary>
    public static ScoreManager instance
    {
        get; private set;
    }

    /// <summary>
    /// Hit率を所得
    /// </summary>
    public float getHitParsent
    {
        get { return (_killedEnemyCount / (float)_shotCount) * 100; }
    }

    /// <summary>
    /// Scoreを所得
    /// </summary>
    /// <returns></returns>
    public int getScore()
    {
        return _score;
    }

    /// <summary>
    /// 敵を倒した数(当たった数)
    /// </summary>
    public int HitEnemyCount
    {
        get { return _killedEnemyCount; }
    }

    /// <summary>
    /// 時間を所得
    /// </summary>
    public float LifeTime
    {
        get { return _lifeTimeCount; }
    }

    /// <summary>
    /// Enemyの弾と当たった数の所得
    /// </summary>
    public int enemyBulletCount
    {
        get { return _flipEnemyBulletCount; }
    }

    /// <summary>
    /// 被弾数の所得
    /// </summary>
    public int getInpactDamageCount
    {
        get { return _inpactDamageCount; }
    }

    /// <summary>
    /// Scoreを加算する関数
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        _score += score;
    }
    
    public ScoreManager AddScore(EnemyType type)
    {
        AddScore(_gunData[(int)type].score);
        return this;
    }

    public ScoreManager AddScore(SlashSword.SlashPattern type)
    {
        AddScore(_swordData[(int)type].score);
        return this;
    }

    /// <summary>
    /// 弾を撃った数を足していく
    /// </summary>
    /// <returns></returns>
    public ScoreManager AddShotCount()
    {
        _shotCount++;
        return this;
    }

    /// <summary>
    /// ゲーム中の時間を計測
    /// </summary>
    /// <returns></returns>
    public ScoreManager GameTimeCount()
    {
        _lifeTimeCount += Time.unscaledTime;
        return this;
    }

    /// <summary>
    /// 敵に当たった数を足していく
    /// </summary>
    /// <returns></returns>
    public ScoreManager AddHitEnemyCount()
    {
        _killedEnemyCount++;
        return this;
    }

    /// <summary>
    /// 敵の弾にプレイヤーが当たった数を足していく
    /// </summary>
    /// <returns></returns>
    public ScoreManager AddInpactDamageCount()
    {
        _inpactDamageCount++;
        return this;
    }

    /// <summary>
    /// Enemyの弾と当たった数を足していく
    /// </summary>
    /// <returns></returns>
    public ScoreManager AddFlipEnemyBulletCount()
    {
        _flipEnemyBulletCount++;
        return this;
    }

    /// <summary>
    /// 全部0に戻す
    /// </summary>
    public void Reset()
    {
        _lifeTimeCount = 0.0f;
        _shotCount = 0;
        _killedEnemyCount = 0;
        _flipEnemyBulletCount = 0;
        _score = 0;
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //_data = CSVLoader.ScoreDataLoad("/Resources/CSV/test.csv");
    }


    void Update()
    {
    }

}
