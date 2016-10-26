using UnityEngine;
using System.Collections;

public class GunSlowButton : MonoBehaviour {

    [SerializeField]
    SteamVR_TrackedObject _trackedObjectRight;

    [SerializeField]
    SteamVR_TrackedObject _trackedObjectLeft;

    const int MAX_COUNT = 10;

    bool _isPushButtonRight = false;
    int _countRigth = 0;

    bool _isPushButtonLeft = false;
    int _countLeft = 0;

    [SerializeField]
    UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration _v;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SteamVR_Controller.Device deviceRight = SteamVR_Controller.Input((int)_trackedObjectRight.index);
        SteamVR_Controller.Device deviceLeft = SteamVR_Controller.Input((int)_trackedObjectLeft.index);

        if (deviceRight.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _isPushButtonRight = true;
            _countRigth = MAX_COUNT;
        }

        if (deviceLeft.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _isPushButtonLeft = true;
            _countLeft = MAX_COUNT;
        }

        //Debug.Log("右" + deviceRight.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad));
        //Debug.Log("左" + deviceLeft.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad));
        //Debug.Log("両方" + (deviceRight.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && 
        //    deviceLeft.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)));

        if(_countRigth > 0)
        {
            _countRigth--;
            if(_countRigth == 0) { _isPushButtonRight = false; }
        }
        if (_countLeft > 0)
        {
            _countLeft--;
            if (_countLeft == 0) { _isPushButtonLeft = false; }
        }

        if (_isPushButtonRight && _isPushButtonLeft)
        {

            if (SlowMotion._instance.isSlow)
            {
                SlowMotion._instance.ResetSpeed();
                StartCoroutine(SlowEnd());
            }
            else
            {
                SlowMotion._instance.GameSpeed(0.1f);
                StartCoroutine(SlowStart());
            }

            _isPushButtonRight = false;
            _isPushButtonLeft = false;
            _countRigth = 0;
            _countLeft = 0;
        }
    }

    IEnumerator SlowStart()
    {
        while (_v.intensity < 0.8f)
        {
            if (!SlowMotion._instance.isSlow) { break; }
            _v.intensity += 0.1f;
            yield return 0;
        }
    }

    IEnumerator SlowEnd()
    {
        while (_v.intensity > 0.0f)
        {
            if (SlowMotion._instance.isSlow) { break; }
            _v.intensity -= 0.1f;
            yield return 0;
        }
    }
}
