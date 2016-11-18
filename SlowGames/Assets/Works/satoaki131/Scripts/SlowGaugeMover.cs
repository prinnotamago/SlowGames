using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGaugeMover : MonoBehaviour {

    [SerializeField]
    private GameObject[] _gaugeVar = null;

    private int _index = 0;

    private float _test = 0;


    void Start()
    {
        _index = _gaugeVar.Length;
        _test = SlowMotion._instance.slowTimeMax / _index;
        Debug.Log(_index);
        Debug.Log(_test);
    }

    void Update()
    {
        SetGauge();
    }

    void SetGauge()
    {
        for(int i = 0; i < _index; i++)
        {
            if (_gaugeVar[i].activeSelf) continue;
            if(SlowMotion._instance.slowTime > _test * i)
            {
                _gaugeVar[i].SetActive(true);
            }
        }
        for (int i = 0; i < _index; i++)
        {
            if (!_gaugeVar[i].activeSelf) continue;
            if (SlowMotion._instance.slowTime < _test * i)
            {
                _gaugeVar[i].SetActive(false);
            }
        }

    }

}
