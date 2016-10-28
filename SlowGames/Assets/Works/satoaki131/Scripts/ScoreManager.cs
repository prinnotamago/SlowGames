using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private int _score = 0;

    private int _hitEnemyCount = 0;
    private int _shotCount = 0;

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
        get{ return _hitEnemyCount / (float)_shotCount; }
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
    /// 敵に当たった数を足していく
    /// </summary>
    /// <returns></returns>
    public ScoreManager AddHitEnemyCount()
    {
        _hitEnemyCount++;
        return this;
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
        Debug.Log(getHitParsent);
    }

}
