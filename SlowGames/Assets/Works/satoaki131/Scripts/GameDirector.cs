using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType
{
    Gun,
    Sword
}

public class GameDirector : MonoBehaviour {

    [SerializeField]
    private PlayerShot[] _playerShot = null;
    [SerializeField]
    private GenerateManager _generateManager = null;

    private bool _gamePlay = false;

    [SerializeField]
    private float _gameStartTime = 0.0f;

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

    public int displayTime
    {
        get; private set;
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
        GameSet();
        StartCoroutine(GameStartCutIn());
    }

    private IEnumerator GameStartCutIn()
    {
        var time = 0.0f;
        while(time < _gameStartTime)
        {
            time += Time.deltaTime;
            displayTime = (int)(_gameStartTime - time + 1);
            yield return null;
        }
        _gamePlay = true;
        GameSet();
    }

    void Update()
    {
        PlayTimeCount();
    }

    void GameSet()
    {
        for (int i = 0; i < _playerShot.Length; i++)
        {
            _playerShot[i].isStart = _gamePlay;
        }
        _generateManager.isTutorial = _gamePlay;
    }

}
