using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {

    [SerializeField]
    private float _reloadTime = 0.5f;

    private PlayerShot _shot = null;

    SteamVR_TrackedObject _trackedObject;
    SteamVR_Controller.Device _device;

    /// <summary>
    /// trueならリロード中
    /// falseならリロードしてない状態
    /// </summary>
    public bool isReload
    {
        get; private set;
    }

    void Start()
    {
        _shot = GetComponent<PlayerShot>();
        //以下Viveが接続されているときのみ使用する
        if (!SteamVR.active) return;
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (SteamVR.active) { _device = SteamVR_Controller.Input((int)_trackedObject.index); } //Viveが接続されていたら読み込む
        if (_shot.maxBulletsNumbers == _shot.bulletsNumber) return;
        if (isReload) return;
        if((!SteamVR.active && Input.GetKeyDown(KeyCode.R)) ||
            (SteamVR.active && _device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            ) //あとでリロードボタンに変更
        {
            StartCoroutine(ShotReload());
        }
    }

    /// <summary>
    /// リロード中の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotReload()
    {
        var time = 0.0f;
        isReload = true;
        //Audioを追加する(カシャッ)
        AudioManager.instance.play3DSe(gameObject, AudioName.SeName.gunreload1);
        //AudioManager.instance.playSe(AudioName.SeName.gunreload1);
        while(time < _reloadTime)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        isReload = false;
        _shot.bulletsNumber = _shot.maxBulletsNumbers;  //弾の数を最大数に変更する
        //音を追加する(カチッ)
        AudioManager.instance.play3DSe(gameObject, AudioName.SeName.gunreload2);

        //AudioManager.instance.playSe(AudioName.SeName.gunreload2);
    }
}
