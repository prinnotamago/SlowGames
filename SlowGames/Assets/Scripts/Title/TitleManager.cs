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

    [SerializeField]
    private GameObject[] _gunStand = null;

    [SerializeField]
    private GameObject[] _gun = null;

    [SerializeField]
    private GameObject[] _cameraRig = null;

    private PlayerShot[] _playerShot = null;
     
    private Dictionary<State, Action> _stateUpdate = null;
    private State _state = State.Title;

    void Start()
    {
        _stateUpdate = new Dictionary<State, Action>();
        _stateUpdate.Add(State.Title, TitleUpdate);
        _stateUpdate.Add(State.Turtreal, TurtrealUpdate);
        _stateUpdate.Add(State.Wait, () => { });
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

    void TurtrealUpdate()
    {

    }

    IEnumerator TurtrealProduction()
    {
        var time = 0.0f;
        var endTime = 2.0f; 
        for(int i = 0; i < _gun.Length; i++)
        {
            Destroy(_gun[i]);
        }

        _cameraRig[0].SetActive(false);
        _cameraRig[1].SetActive(true);
        foreach(var shot in FindObjectsOfType<PlayerShot>())
        {
            shot.isStart = true;
        }

        while (_spotLights[0].intensity != 0)
        {
            time += Time.unscaledDeltaTime;
            for(int i = 0; i < _spotLights.Length; i++)
            {
                _spotLights[i].intensity = Mathf.Lerp(_spotLights[i].intensity, 0, time / endTime);
            }
            yield return null;
        }
        for(int i = 0; i < _gunStand.Length; i++)
        {
            Destroy(_gunStand[i]);
        }
        //_color.EnableKeyword("_EMISSION");
        //_color.SetColor("_EmissionColor", _currentColor);
    }

}
