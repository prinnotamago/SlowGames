using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranker : MonoBehaviour {

    [SerializeField] //Debug用、シリアライズはあ後で消します
    private int _lastScore = 0; //最終スコア

    [SerializeField]
    private Text _text = null;

    [System.Serializable]
    public struct RankData
    {
        public int score;
        public string rankName;
    }

    [SerializeField, Tooltip("ランクに合わせたスコアを入れる")]
    private RankData[] _data;

    void Start()
    {
        //_lastScore = ScoreManager.instance.getScore(); //最後はコメント外す
        for(int i = 0; i < _data.Length; i++)
        {
            if(_lastScore > _data[i].score) { continue; }
            _text.text =  _data[i].rankName;
            break;
        }
    }

    void Update()
    {

    }
}
