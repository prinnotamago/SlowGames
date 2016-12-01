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

    [SerializeField]
    private Text[] _text = null;

    private char[] _displayPassNumber = null; //IDの数字列を１つずつ分解
    private int _randomNumber = 0; //ランダム演出の数字
    private int END_INDEX = 6; //配列の最後尾から止めていく
    private float _time = 0.0f; //タイムをカウントしていく
    [SerializeField]
    private float _stopTime = 2.0f; //一つ目の数字の止めるボタン

    void Start()
    {
        _passward = UnityEngine.Random.Range(MIN_NUMBER, MAX_NUMBER  + 1); //最初にIDをランダムで決める
        _text = GetComponentsInChildren<Text>(); //Textを検索
        _displayPassNumber = _passward.ToString().ToCharArray(); //Display用にIDを1つずつ分解
    }

    void Update()
    {
        _time += Time.unscaledDeltaTime; 
        for (int i = 0; i < _text.Length; i++)
        {
            if (i >= END_INDEX) break;
            _randomNumber = UnityEngine.Random.Range(0, 10);
            _text[i].text = _randomNumber.ToString();

            //Timeが一定越えたら配列の一番後ろから止めていく
            if (_time > _stopTime)
            {
                _stopTime += 0.6f;
                END_INDEX--;
                if (END_INDEX < 0) break;
                _text[END_INDEX].text = _displayPassNumber[END_INDEX].ToString();
            }
        }
    }
}