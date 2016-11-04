using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private int _score = 0;

    private int _killedEnemyCount = 0;
    private int _shotCount = 0;

    private float _lifeTimeCount = 0.0f;

    private int _flipEnemyBulletCount = 0;

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
        get{ return (_killedEnemyCount / (float)_shotCount) * 100; }
    }

    /// <summary>
    /// 敵を倒した数(当たった数)
    /// </summary>
    public int HitEnemyCount
    {
        get { return _killedEnemyCount; }
    }

    public float LifeTime
    {
        get{ return _lifeTimeCount; }
    }

    //後でソード用にも書き換え
    public ScoreManager AddScore(GameType gameType, EnemyType type)
    {
        switch(gameType)
        {
            case GameType.Gun:
                _score += _gunData[(int)type].score;
                //_score += _data[(int)type].score;
                break;
            case GameType.Sword:
                _score += _swordData[(int)type].score; //後で書き換え(配列の番号)
                break;
        }
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
        if(instance == null)
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
