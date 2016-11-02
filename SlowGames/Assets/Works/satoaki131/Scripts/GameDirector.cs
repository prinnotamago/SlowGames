using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour {

    private bool _gamePlay = false;

    /// <summary>
    /// ゲーム中かどうか
    /// trueならGameが動いてる状態
    /// falseならGameが止まってる状態
    /// </summary>
    public bool isPlayGame
    {
        get{ return _gamePlay; }
        set { _gamePlay = value; }
    }

    /// <summary>
    /// instanceの所得
    /// </summary>
    public static GameDirector instance
    {
        get; private set;
    } 

    /// <summary>
    /// 時間計測する関数
    /// </summary>
    void PlayTimeCount()
    {
        if (!_gamePlay) return;
        ScoreManager.instance.GameTimeCount();
    }

    //////////////////////////////////////////////////////////////////////

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        PlayTimeCount();
    }

}
