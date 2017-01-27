using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMoveAnim : MonoBehaviour {

    private Animator _anim = null;

    public bool isholdGun
    {
        get; private set;
    }

    [SerializeField]
    SteamVR_TrackedObject _trackedObject;
    SteamVR_Controller.Device _device;

    void Start()
    {
        isholdGun = false;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (SteamVR.active) { _device = SteamVR_Controller.Input((int)_trackedObject.index); }
        if (_anim != null)
        {
            float value = _device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x; //トリガーのニュウリョクの深さを0～1で受け取る
            SetAnimFrame(value); //Animationの決定
        }
    }

    void SetAnimFrame(float frame)
    {
        var animationHash = _anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
        _anim.Play(animationHash, 0, frame);
    }

    public void setGunState(bool value)
    {
        isholdGun = value;
    }
}
