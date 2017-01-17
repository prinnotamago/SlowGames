using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{

    public enum State
    {
        GunPut,
        End,
        Wait
    }
    private Dictionary<State, Action> _stateUpdate = null;
    private State _state = State.GunPut;

    [SerializeField]
    private PutGunStand[] _put = null;

    [SerializeField]
    private Image _logo = null;

    [SerializeField] //仮置き
    private TextMessage _thankyouText = null;

    [SerializeField, Range(2.0f, 6.0f)]
    private float _logoMoveEndTime = 2.0f;

    void Start()
    {
        _stateUpdate = new Dictionary<State, Action>();
        _stateUpdate.Add(State.GunPut, GunPutUpdate);
        _stateUpdate.Add(State.Wait, () => { });
        _stateUpdate.Add(State.End, EndUpdate);
    }

    void Update()
    {
        //デバッグ用:台座に置いてないときでも戻れるように
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneChange.ChangeScene(SceneName.Name.Title, Color.white);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            _state = State.Wait;
            StartCoroutine(Production());
        }
        _stateUpdate[_state]();
    }

    void GunPutUpdate()
    {
        if (!_put[0].isPutGun) return;
        if (!_put[1].isPutGun) return;

        _state = State.Wait;
        StartCoroutine(Production());
    }

    private IEnumerator Production()
    {
        //thank you for playing演出
        _thankyouText.isMoveText = true;
        while(!_thankyouText.isPopText)
        {
            yield return null;
        }

        //Logo演出
        var time = 0.0f;
        while (time < _logoMoveEndTime)
        {
            time += Time.unscaledDeltaTime;
            _logo.fillAmount = (float)Easing.InOutQuad(time, _logoMoveEndTime, 1.0f * 2, 0.0f);
            yield return null;
        }

        //ざらざら演出
        time = 0.0f;
        while(time < _logoMoveEndTime)
        {
            time += Time.unscaledDeltaTime;
            NoiseSwitch.instance.noise.intensityMultiplier = (float)Easing.OutCubic(time, _logoMoveEndTime * 2, 10.0f, 0.0f);
            yield return null;
        }

        yield return null;
        _state = State.End;
    }

    void EndUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ChangeScene(SceneName.Name.Title, Color.white);
        }
    }


}
