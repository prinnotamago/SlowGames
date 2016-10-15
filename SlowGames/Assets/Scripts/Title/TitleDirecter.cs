using UnityEngine;
using System.Collections;

public class TitleDirecter : MonoBehaviour {

    private SteamVR_TrackedObject _trackedObject;
    private SteamVR_Controller.Device _device;

    private bool _collition = false;

    void Start()
    {
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        _device = SteamVR_Controller.Input((int)_trackedObject.index);
        Debug.Log(_collition);

        if (!_collition) return;
        if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            SceneChange.ChangeScene(SceneName.Name.MainGame);
        }


    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("Stay");
        if (col.gameObject.tag == "UI")
        {
            _collition = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Enter");
        if (col.gameObject.tag == "UI")
        {
            _collition = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Exit");
        if (col.gameObject.tag == "UI")
        {
            _collition = false;
        }
    }
}
