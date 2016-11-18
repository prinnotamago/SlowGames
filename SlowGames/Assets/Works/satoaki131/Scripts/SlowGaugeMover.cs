using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGaugeMover : MonoBehaviour {

    [SerializeField]
    private GameObject[] _gaugeVar = null;

    private int _gaugeVarLength = 0; //_gaugeVarの配列の長さ

    private float _equallyDividedGauge = 0; //Lengthで等分した値


    void Start()
    {
        _gaugeVarLength = _gaugeVar.Length;
        _equallyDividedGauge = SlowMotion._instance.slowTimeMax / _gaugeVarLength;
    }

    void Update()
    {
        SetGauge();
    }

    /// <summary>
    /// ゲージの量を切り替える
    /// </summary>
    void SetGauge()
    {
        for(int i = 0; i < _gaugeVarLength; i++)
        {
            if (_gaugeVar[i].activeSelf) continue;
            if(SlowMotion._instance.slowTime > _equallyDividedGauge * i)
            {
                _gaugeVar[i].SetActive(true);
            }
        }
        for (int i = 0; i < _gaugeVarLength; i++)
        {
            if (!_gaugeVar[i].activeSelf) continue;
            if (SlowMotion._instance.slowTime < _equallyDividedGauge * i)
            {
                _gaugeVar[i].SetActive(false);
            }
        }

    }

}
