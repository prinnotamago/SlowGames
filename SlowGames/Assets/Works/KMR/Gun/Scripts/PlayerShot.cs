
using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Reload))]
public class PlayerShot : MonoBehaviour
{
    Reload _reload;

    [SerializeField]
    GameObject _mazzleFlush;

    [SerializeField]
    GameObject _bullet;

    Recoil _recoil;

    [SerializeField]
    Animator _ringAnim = null;

    [SerializeField]
    int _maxBulletsNumbers = 20;

    [SerializeField]
    private Animator _triggerAnim = null;

    int _reloadHash;

    bool _isReload;

    int _bulletsNumber;

    int _ringShotHash;


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

    //銃にのリロードするかしないかの状態
    enum shotType
    {
        notReload,
        autoReload
    }

    bool _isShot = false;

    public bool isShot
    {
        get { return _isShot; }
    }

    int _burstCount;

    AimAssist _aimAssist;

    SteamVR_TrackedObject _trackedObject;
    SteamVR_Controller.Device _device;

    [SerializeField]
    shotType _shotType = shotType.autoReload;

    [SerializeField]
    Animator _gunAnim = null;

    bool _isStart = false;

    AudioSource _bulletShotSe = null;

    public bool isStart
    {
        get { return _isStart; }
        set { _isStart = value; }
    }

    int _reShotHash;
    bool _reShot = false;

    void Start()
    {
        _isReload = false;
        _reloadHash = Animator.StringToHash("isReload");
        _reShotHash = Animator.StringToHash("reShot");
        _ringShotHash = Animator.StringToHash("isShotRing");
        _recoil = GetComponent<Recoil>();
        _aimAssist = GetComponentInChildren<AimAssist>();
        _bulletsNumber = _maxBulletsNumbers;
        _reload = GetComponent<Reload>();
        if (!SteamVR.active) return;
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
        _bulletShotSe = AudioManager.instance.getSe(AudioName.SeName.BossPurge);
    }

    void Update()
    {
        if (SteamVR.active) { _device = SteamVR_Controller.Input((int)_trackedObject.index); }

        if (!_isStart) return;
        ReloadAnim();

        if (_reload.isReload)
        {
            if (_isShot) _isShot = false;
            return;
        }

        if (_reShot)
        {
            return;
        }
        if (_gunAnim != null)
        {
            float value = _device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x; //トリガーのニュウリョクの深さを0～1で受け取る
            SetAnimFrame(value); //Animationの決定
        }

        if (SlowMotion._instance.limiterFlag)
        {
            _shotType = shotType.notReload;
        }

        BulletCreate();

        if (!SteamVR.active && !Input.GetKeyDown(KeyCode.A) ||
            SteamVR.active && !_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) { return; }

        _aimAssist.OrientationCorrection();
        _isShot = true;
    }

    //弾の生成（弾の向きを決めている）
    void BulletCreate()
    {
        if (_shotType == shotType.autoReload)
        {
            if (_bulletsNumber <= 0) return;
        }

        if (_isShot == false) return;
        _gunAnim.speed = 1.0f;
        _ringAnim.speed = 1.0f;

        _bulletShotSe.Play();

        if (SteamVR.active)
        {
            _device.TriggerHapticPulse(4000);
        }
        GameObject shotBullet = Instantiate(_bullet);
        if (_aimAssist.enemyHit == false)
        {
            shotBullet.transform.rotation = transform.rotation;
            shotBullet.GetComponent<Shot>().direction = transform.forward - transform.up;
        }
        else
        if (_aimAssist.enemyHit == true)
        {
            shotBullet.transform.rotation = transform.rotation;
            shotBullet.GetComponent<Shot>().direction = _aimAssist.enemyDirection;
        }

        _reShot = true;
        //_gunAnim.speed = 1.0f;
        _gunAnim.SetBool(_reShotHash, _reShot);
        _ringAnim.SetBool(_ringShotHash, _reShot);
        StartCoroutine(ShotInterval());


        //弾の発生位置変更
        shotBullet.transform.position = transform.position + transform.forward * 0.4f - transform.up * 0.4f;
        var effect = Instantiate(_mazzleFlush);
        effect.transform.position = transform.position;
        effect.transform.LookAt(shotBullet.transform);
        effect.transform.position = transform.position + transform.forward * 0.2f - transform.up * 0.2f;

        _burstCount--;
        if (_shotType == shotType.autoReload)
        {
            _bulletsNumber--;
        }
        _isShot = false;
    }

    //撃った時のアニメーションのインターバルの処理
    private IEnumerator ShotInterval()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        _reShot = false;
        _gunAnim.SetBool(_reShotHash, _reShot);
        _ringAnim.SetBool(_ringShotHash, _reShot);
        yield return null;
    }

    //手のアニメーションをトリガーの入力の深さに対して動かしている処理
    void SetAnimFrame(float frame)
    {
        if (!_gunAnim.GetCurrentAnimatorStateInfo(0).IsName("Wait")) return;
        var animationHash = _gunAnim.GetCurrentAnimatorStateInfo(0).shortNameHash;
        _gunAnim.Play(animationHash, 0, frame);
    }


    //リロードのアニメーションの処理
    void ReloadAnim()
    {
        if (_reload.isReload)
        {
            _isReload = true;
            _gunAnim.speed = 10.0f * SlowMotion._instance.RealSpeed();
            _ringAnim.speed = 10.0f * SlowMotion._instance.RealSpeed(); ;
        }
        else
        if (!_reload.isReload)
        {
            _gunAnim.speed = 10.0f * SlowMotion._instance.RealSpeed();
            _ringAnim.speed = 10.0f * SlowMotion._instance.RealSpeed();
            _isReload = false;

        }
        _gunAnim.SetBool(_reloadHash, _isReload);

    }
}
