using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private int _score = 0;

    private int _hitEnemyCount = 0;
    private int _shotCount = 0;

    private float _lifeTimeCount = 0.0f;

    private int _flipEnemyBulletCount = 0;

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
        get{ return (_hitEnemyCount / (float)_shotCount) * 100; }
    }

    /// <summary>
    /// 敵を倒した数(当たった数)
    /// </summary>
    public int HitEnemyCount
    {
        get { return _hitEnemyCount; }
    }

    public float LifeTime
    {
        get{ return _lifeTimeCount; }
    }

    public ScoreManager AddScore()
    {
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
        _hitEnemyCount++;
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
        _hitEnemyCount = 0;
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
    }


    void Update()
    {
    }

}
