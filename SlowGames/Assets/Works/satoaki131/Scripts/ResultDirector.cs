using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDirector : MonoBehaviour {

    [SerializeField]
    private Text _hitPersent = null;

    [SerializeField]
    private Text _time = null;

    [SerializeField]
    private Text _inpactDamageCount = null;

    [SerializeField]
    private Text _score = null;

    void Start()
    {
        var resultSecondTime = (int)ScoreManager.instance.LifeTime % 60;
        var resultMinuteTime = (int)ScoreManager.instance.LifeTime / 60;

        _hitPersent.text = "ヒット率： " + ScoreManager.instance.getHitParsent + " ％";
        _time.text = "時間： " + resultMinuteTime + "：" + resultSecondTime.ToString().PadLeft(2, '0');
        _inpactDamageCount.text = "被弾数： " + ScoreManager.instance.getInpactDamageCount;
        _score.text = "スコア： " + ScoreManager.instance.getScore();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ChangeScene(SceneName.Name.Title);
        }
    }

}
