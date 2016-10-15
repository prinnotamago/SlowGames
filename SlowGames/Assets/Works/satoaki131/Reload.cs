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
        if (!SteamVR.active) return;
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (SteamVR.active) { _device = SteamVR_Controller.Input((int)_trackedObject.index); }
        if (_shot.MaxBulletsNumbers == _shot.BulletsNumber) return;
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
        while(time < _reloadTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isReload = false;
        //音を追加する(カチッ)
    }
}
