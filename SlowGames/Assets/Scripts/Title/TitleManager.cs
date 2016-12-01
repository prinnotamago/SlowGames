using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleManager : MonoBehaviour {

    enum State
    {
        Title,
        Turtreal,
        Wait
    }

    /// <summary>
    /// シーンチェンジに必要なアイテム
    /// </summary>
    [SerializeField]
    ViveGrab[] _items;

    [SerializeField]
    private Light[] _spotLights = null; 

    private Dictionary<State, Action> _stateUpdate = null;
    private State _state = State.Title;

    void Start()
    {
        _stateUpdate = new Dictionary<State, Action>();
        _stateUpdate.Add(State.Title, TitleUpdate);
    }

    void Update()
    {
        _stateUpdate[_state]();
    }

    void TitleUpdate()
    {
        // 必要なアイテムを手に持っているか確かめる
        bool isChange = true;
        foreach (var item in _items)
        {
            // 持ってないなら
            if (!item.isPick)
            {
                isChange = false;
            }
        }

        // 持っていたらシーンを変える
        if (isChange)
        {
            _state = State.Turtreal;
            StartCoroutine(TurtrealProduction());
        }
    }

    IEnumerator TurtrealProduction()
    {
        var time = 0.0f;
        while(_spotLights[0].intensity == 0)
        {
            time += Time.unscaledDeltaTime;
            for(int i = 0; i < _spotLights.Length; i++)
            {
                _spotLights[i].intensity = Mathf.Lerp(_spotLights[i].intensity, 0, time);
            }
            yield return null;
        }
        Debug.Log("OK");
    }

}
