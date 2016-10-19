using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルの管理スクリプト
/// シーン遷移とかやってる
/// </summary>
public class TitleDirecter : MonoBehaviour {

    private SteamVR_TrackedObject _trackedObject;
    private SteamVR_Controller.Device _device;

    private bool _collition = false; //遷移するためのオブジェクトに触れているかどうか

    void Start()
    {
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        _device = SteamVR_Controller.Input((int)_trackedObject.index);

        if (!_collition) return;
        if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            SceneChange.ChangeScene(SceneName.Name.Select);
        }


    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "UI")
        {
            _collition = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "UI")
        {
            _collition = false;
        }
    }
}
