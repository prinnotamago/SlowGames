﻿
using UnityEngine;
[RequireComponent(typeof(Reload))]
public class PlayerShot : MonoBehaviour
{
    Reload _reload;

    [SerializeField]
    GameObject _mazzleFlush;

    [SerializeField]
    GameObject _bullet;

    [SerializeField]
    int _oneShotCount = 3;

    [SerializeField]
    float _vibrationPower = 200;

    [SerializeField]
    float _burstIntervalTime = 0.2f;


    [SerializeField]
    int _maxBulletsNumbers = 20;

    int _bulletsNumber;

    public int maxBulletsNumbers
    {
        get { return _maxBulletsNumbers; }
        set { _maxBulletsNumbers = value; }
    }

    public int bulletsNumber
    {
        get { return _bulletsNumber; }
        set { _bulletsNumber = value; }
    }


    bool _isShot = false;

    float _time;

    int _burstCount;

    AimAssist _aimAssist;

    SteamVR_TrackedObject _trackedObject;
    SteamVR_Controller.Device _device;
    //GameObject steamVR_Camera;
    //Vector2 position;

    void Start()
    {
        _aimAssist = GetComponentInChildren<AimAssist>();
        _bulletsNumber = _maxBulletsNumbers;
        _reload = GetComponent<Reload>();
        _burstCount = _oneShotCount;
        _time = _burstIntervalTime;
        if (!SteamVR.active) return;
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
        //steamVR_Camera = FindObjectOfType<SteamVR_Camera>().gameObject;
    }

    void Update()
    {
        if (SteamVR.active) { _device = SteamVR_Controller.Input((int)_trackedObject.index); }
        if (_reload.isReload)
        {
            if(_isShot)_isShot = false;
            return;
        }
        ThreeBurst();
        if (!SteamVR.active && !Input.GetKeyDown(KeyCode.A) ||
            SteamVR.active && !_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) { return; }
        //if (!Input.GetKeyDown(KeyCode.A)) { return; }
        _aimAssist.OrientationCorrection();
        _isShot = true;
        _burstCount = _oneShotCount;
    }

    void ThreeBurst()
    {

        if (_bulletsNumber <= 0) return;
        if (_isShot == false) return;

        _time -= Time.unscaledDeltaTime;
        if (_time > 0) return;

        //int vibration = 200 * i;
        AudioManager.instance.playSe(AudioName.SeName.gun1);
        var effect = Instantiate(_mazzleFlush);
        effect.transform.rotation = transform.rotation;
        effect.transform.position = transform.position;
        effect.transform.Translate(0, -1, 1);
        effect.transform.rotation = Quaternion.Euler(0, 180, 0);
        if (SteamVR.active)
        {
            _device.TriggerHapticPulse(4000);
        }
        GameObject shotBullet = Instantiate(_bullet);
        
        if (_aimAssist.enemyHit == false)
        {
            shotBullet.transform.rotation = transform.rotation;
            shotBullet.GetComponent<Shot>().Start(shotBullet.transform.forward - shotBullet.transform.up);
        }
        else
        if(_aimAssist.enemyHit == true)
        {
            shotBullet.transform.rotation = transform.rotation;
            shotBullet.GetComponent<Shot>().Start(_aimAssist.enemyDirection);
        }
        //Shotbullet.transform.Rotate(45,0,0);
        //弾の発生位置変更
        //            Shotbullet.transform.position = transform.position;
        shotBullet.transform.position = transform.position + transform.forward - transform.up;
        //Shotbullet.transform.Translate(0, -1, 1);

        _time = _burstIntervalTime;
        _burstCount--;
        _bulletsNumber--;
        if (_burstCount < 1)
        {
            _burstCount = _oneShotCount;
            _isShot = false;
        }
    }

}