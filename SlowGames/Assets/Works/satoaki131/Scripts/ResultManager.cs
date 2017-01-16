using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
    private Canvas _logoCanvas = null;

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
        //thank you for playingとlogo演出
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
