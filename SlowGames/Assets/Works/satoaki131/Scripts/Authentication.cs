using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザー認証スクリプト
/// </summary>
public class Authentication : MonoBehaviour {

    [SerializeField]
    private int MIN_NUMBER = 0;
    [SerializeField]
    private int MAX_NUMBER = 0;

    //表示するID
    private int _passward = 0;

    private Text[] _text = null;

    private char[] _displayPassNumber = null; //IDの数字列を１つずつ分解
    private int _randomNumber = 0; //ランダム演出の数字
    private int END_INDEX = 0; //配列の最後尾から止めていく
    private float _time = 0.0f; //タイムをカウントしていく
    [SerializeField]
    private float _stopTime = 2.0f; //一つ目の数字の止めるボタン

    private bool _authenticationEnd = false;

    [SerializeField]
    private Text[] _changeText = null;

    void Start()
    {
        _passward = UnityEngine.Random.Range(MIN_NUMBER, MAX_NUMBER  + 1); //最初にIDをランダムで決める
        _text = GetComponentsInChildren<Text>(); //Textを検索
        _displayPassNumber = _passward.ToString().ToCharArray(); //Display用にIDを1つずつ分解
    }

    void Update()
    {
        _time += Time.unscaledDeltaTime; //時間を計測
        for (int i = 0; i < _text.Length; i++)
        {
            if (_authenticationEnd) break; //全部表示されたらfor文抜ける
            if (i < END_INDEX) continue; //Random演出が終わったindexはfor文を回さない
            _randomNumber = UnityEngine.Random.Range(0, 10); //ランダム演出用の数字を決める
            _text[i].text = _randomNumber.ToString(); //いったんRandomの数字を表示

            //Timeが一定越えたら配列の先頭から止めていく
            if (_time > _stopTime)
            {
                _stopTime += 0.6f; //次の数字を止めるまでの時間を追加
                END_INDEX++; //演出が終わってるindex番号を一つ増やす
                _text[i].text = _displayPassNumber[i].ToString(); //表示する数字を代入
                if (END_INDEX == _text.Length && !_authenticationEnd) { _authenticationEnd = true; } //全部終わったらfor文を回さないようにするboolをtrueにする
            }
        }

        if (!_authenticationEnd) return;
        if (!_changeText[0].gameObject.activeSelf) { return; }

        //全ての演出が終わったら「認証官僚」の文字に切り替える

        _changeText[0].gameObject.SetActive(false);
        _changeText[1].gameObject.SetActive(false);
        _changeText[2].gameObject.SetActive(true);


    }
}