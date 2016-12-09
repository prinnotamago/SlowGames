using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainGame,
    Result
}

public class GameDirector : MonoBehaviour {

    [SerializeField]
    private PlayerShot[] _playerShot = null;
    [SerializeField]
    private GenerateManager _generateManager = null;

    private bool _gamePlay = false;

    [SerializeField]
    private float _gameStartTime = 0.0f;

    private PlayerHP _hp = null;

    [SerializeField]
    private Light _pointLight = null;

    [SerializeField]
    private Canvas _resultCanvas = null;

    [SerializeField]
    private Image _gameClearImage = null;

    [SerializeField]
    private GameObject _boss = null;

    private Dictionary<GameState, Action> _update = null;

    private GameState _state = GameState.MainGame;

    private bool _isBossDestroy = false;

    /// <summary>
    /// ボスが死んだときに呼ぶ関数
    /// </summary>
    public void isBossDestroy()
    {
        _isBossDestroy = true;
    }

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
        _update = new Dictionary<GameState, Action>();
        _update.Add(GameState.MainGame,MainGameUpdate);
        _update.Add(GameState.Result,() => { });
        GameSet();
        StartCoroutine(GameStartCutIn());
        _hp = FindObjectOfType<PlayerHP>();
    }

    [SerializeField]
    int _clearDeathCount = 30;

    public int clearEnemyKillCount
    {
        get { return _clearDeathCount; }
    }

    private void MainGameUpdate()
    {
        PlayTimeCount();

        if (_hp.PlayerHp <= 0 && _gamePlay)
        {
            _gamePlay = false;
            GameSet();
            StartCoroutine(ResultChangeStage(_hp.gameOverImage));
        }
        else if(_generateManager._deathCount == _clearDeathCount)
        {
            _clearDeathCount--;
            Instantiate(_boss);
        }
        else if(_isBossDestroy && _gamePlay)
        {
            SceneChange.ChangeScene(SceneName.Name.Result, Color.white);
            //_gamePlay = false;
            //GameSet();
            //StartCoroutine(ResultChangeStage(_gameClearImage));
        }

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
        _update[_state]();
    }

    private IEnumerator ResultChangeStage(Image activeImage)
    {
        var time = 0.0f;
        activeImage.gameObject.SetActive(true);
        while(_pointLight.intensity > 0)
        {
            time += Time.unscaledDeltaTime;
            RenderSettings.ambientIntensity = Mathf.Lerp(1, 0, time / 2.0f);
            _pointLight.intensity = Mathf.Lerp(1, 0, time / 2.0f);
            yield return null;
        }

        _state = GameState.Result;
        _generateManager.DestroyAllEnemy();
        activeImage.gameObject.SetActive(false);
        _resultCanvas.gameObject.SetActive(true);
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
