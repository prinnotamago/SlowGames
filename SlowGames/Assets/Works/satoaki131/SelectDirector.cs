using UnityEngine;
using System.Collections;

public class SelectDirector : MonoBehaviour
{
    const int GUN_NUMBER = 0;
    const int FIST_NUMBER = 1;
    const int SWORD_NUMBER = 2;

    private bool _collision = false;

    [SerializeField, Tooltip("0:Gun, 1:Fist, 2:Sword")]
    private GameObject[] _candidateObj = null;

    private GameObject _selectObj = null;

    private SteamVR_TrackedObject _trackedObject = null;
    private SteamVR_Controller.Device _device = null;

    void Start()
    {
        if (!SteamVR.active) return;
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (SteamVR.active) { _device = SteamVR_Controller.Input((int)_trackedObject.index); }
        if (!_collision) return;
        if (_selectObj == null) return;
        if (!_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) return;
        if(_selectObj == _candidateObj[GUN_NUMBER]) //Gun
        {
            SceneChange.ChangeScene(SceneName.Name.MainGame);
        }
        else if(_selectObj == _candidateObj[FIST_NUMBER]) //Fist
        {
            SceneChange.ChangeScene(SceneName.Name.FistGame);
        }
        else if(_selectObj == _candidateObj[SWORD_NUMBER]) //Sword
        {
            SceneChange.ChangeScene(SceneName.Name.SwordGame);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "UI")
        {
            _collision = true;
            _selectObj = col.gameObject;
            Debug.Log(_selectObj);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "UI")
        {
            _collision = false;
            _selectObj = null;
        }
    }
}
